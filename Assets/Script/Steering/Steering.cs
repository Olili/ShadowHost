using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour {

    static readonly float slowingRadius = 2;

    protected Rigidbody rb;
    [HideInInspector] public Vector3 OnPlanNormal;
    protected Vector3 steering;
    Puppet puppet;
    Vector3 computedVelocity;
    //// better for OnDrawGizmos : 
    //protected Vector3 steeringGizmo;
    //Ray avoidRayDebug;

    public void Awake()
    {
        steering = Vector3.zero;
        puppet = GetComponent<Puppet>();
        rb = GetComponent<Rigidbody>();
    }
   
    // Attention Cette fonction ne doit être appellée qu'une fois par fixedUpdate.
    public Vector3 ComputedVelocity
    {
        get
        {
            if (steering == Vector3.zero)
                return rb.velocity;

            float ySave = 0; // save y, pour ne pas écraser le fait que l'on tombe
            ySave = rb.velocity.y;
            //On clamp pour que max == acceleration
            steering = Vector3.ClampMagnitude(steering, puppet.stats.Get(Stats.StatType.move_speed));
            // On vire la composante y
            Vector3 velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            // On ajoute l'acceleration
            //velocity += (steering  * puppet.stats.Get(Stats.StatType.maxAcceleration));
            velocity += (steering * 50 * Time.fixedDeltaTime * puppet.stats.Get(Stats.StatType.maxAcceleration));
            // On clamp à maxSpeed
            velocity = Vector3.ClampMagnitude(velocity, puppet.stats.Get(Stats.StatType.move_speed));
            // On recupère la composante y de base
            computedVelocity = new Vector3(velocity.x, ySave + velocity.y, velocity.z);
            //Debug.Log("ComputedVelocity :" + computedVelocity);
            steering = Vector3.zero;
            //Debug.Log("CVel :" + computedVelocity);
            return computedVelocity;
        }
    }
    public void FixedUpdate()
    {
        GroundGravityCheck();
    }

#region Basic steering Movement
    public void Seek(Vector3 target)
    {
        Vector3 desiredVelocityPlan = GetDvOnPlan(target, OnPlanNormal);
        Vector3 force = desiredVelocityPlan - rb.velocity;

        force = Vector3.ClampMagnitude(force, puppet.stats.Get(Stats.StatType.move_speed));
        steering += force;
    }
    public void Flee(Vector3 target)
    {
        Vector3 desiredVelocityPlan = -GetDvOnPlan(target, OnPlanNormal);
        Vector3 force = desiredVelocityPlan - rb.velocity;

        force = Vector3.ClampMagnitude(force, puppet.stats.Get(Stats.StatType.move_speed));
        steering += force;

    }
    public void Arrival(Vector3 target)
    {
        Vector3 desiredVelocityPlan = GetDvOnPlan(target, OnPlanNormal);
        float distance = desiredVelocityPlan.magnitude;

        if (desiredVelocityPlan.magnitude < slowingRadius)
        {
            desiredVelocityPlan = desiredVelocityPlan * (distance / slowingRadius);
        }
        Vector3 force = desiredVelocityPlan - rb.velocity;
        force = Vector3.ClampMagnitude(force, puppet.stats.Get(Stats.StatType.move_speed));
         steering += force;
    }
    public void Pursuit(Vector3 target, Vector3 targetVelocity)
    {
        float T = 0.5f;
        Vector3 futurePosition = target + targetVelocity * T;
        Seek(futurePosition);

    }

    public void Evade(Vector3 target, Vector3 targetVelocity)
    {
        float T = 0.5f;
        Vector3 futurePosition = target + targetVelocity * T;
        Flee(futurePosition);

    }
    float wanderAngle = 0;

    

    public void Wander()
    {
        float wanderT = 30;
        float circleRadius = 1;
        float circleDistance = 2;
        Vector3 circleCenter = transform.position + rb.velocity.normalized * circleDistance;

        wanderAngle += Random.Range(-wanderT, wanderT);
        Vector3 circleOffset = new Vector3(Mathf.Cos(wanderAngle * Mathf.Deg2Rad), 0, Mathf.Sin(wanderAngle * Mathf.Deg2Rad)) * circleRadius;
        Vector3 target = circleCenter + circleOffset;
        Seek(target);
        /*
            Je choisis une direction random. 
            Je cree un cercle ayant pour centre la direction actuelle.
            et un rayon arbitraire. 
            Je choisis un point aléatoire dans ce cercle. 
            Je décide de seek vers cette direction. 
            Ou
            Je choisis une direction random. 
            Je cree un cercle ayant pour centre la direction actuelle.
            et un rayon arbitraire. 
            Je choisis un point aléatoire dans ce cercle. 
            Ma direction actuelle devient cette direction. 
        */

    }
    #endregion
    #region Advanced Steering Movement 
    // Peut être améliorer : Lorsqu'une unité est un milieu d'un gros groupe elle est coincée et vibre.
    // de plus ici on push dans une sphere autour de nous.  Mais ce qui compte c'est surtout d'éviter de vers quoi on avance.
    public void Separation()
    {
        
        int separationMask = LayerMask.GetMask(new string[] { "Puppet" });
        Collider[] puppetsCollided;
        Vector3 separationForce = Vector3.zero;
        float sphereCheckRadius = puppet.Extents.magnitude * 1.8f;
        puppetsCollided = Physics.OverlapSphere(transform.position, sphereCheckRadius, separationMask);
        if (puppetsCollided != null)
        {
            int i;
            for (i = 0; i < puppetsCollided.Length; i++)
            {
                if (puppetsCollided[i].transform != this.transform)
                {
                    Vector3 vecFromOther = puppet.transform.position - puppetsCollided[i].transform.position;
                    float distance = vecFromOther.magnitude;
                    vecFromOther.Normalize();
                    if (distance!=0)
                    {
                        vecFromOther = vecFromOther * sphereCheckRadius / distance;
                        separationForce += vecFromOther * 2;
                    }
                }
            }
            separationForce = Vector3.ClampMagnitude(separationForce, puppet.stats.Get(Stats.StatType.move_speed));
            steering += separationForce;
        }
    }
    public void LeaderFollowing(Vector3 target, Vector3 targetVelocity,Vector3 targetDirection)
    {
        Vector3 behind;
        float followingDistance = 1f;
        if (targetVelocity.magnitude> followingDistance)
        {
            behind = target + (targetVelocity.normalized * -followingDistance);
        }
        else
            behind = target + (targetDirection.normalized * -followingDistance);

        Evade(target, targetVelocity);
        Arrival(behind);

    }
    #endregion

    protected virtual void GroundGravityCheck()
    {
        // peut être optimi en changeant layer + pas à chaques frames.
        RaycastHit hit;
        if (Physics.Raycast(transform.position+Vector3.up, -2*Vector3.up, out hit))
        {
            OnPlanNormal = hit.normal;
        }
    }
    public virtual Vector3 GetDvOnPlan(Vector3 target, Vector3 planNormal)
    {
        Vector3 dV = (target - transform.position);
        dV = Vector3.ClampMagnitude(dV, puppet.stats.Get(Stats.StatType.move_speed));
        return dV;

            //// pour le moment pas besoin de marcher sur des trucs en pente : 
        //float distance = dV.magnitude;
        //dV.Normalize();
        //Vector3 right = Vector3.Cross(dV, planNormal);
        //Vector3 planDv = Vector3.Cross(planNormal, right);

        //if (distance > puppet.stats.Get(Stats.StatType.move_speed))
        //    return planDv.normalized * puppet.stats.Get(Stats.StatType.move_speed); 
        //else
        //    return planDv.normalized * distance;

    }
}
