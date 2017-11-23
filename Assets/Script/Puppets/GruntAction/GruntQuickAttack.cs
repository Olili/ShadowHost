using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntQuickAttack : GruntAction
{
    float timer;
    float chargeTime ;
    Vector3 attackExtents;
    Vector3 attackOrigin;
    float pushValue;
    bool attackStarted = false;


    public GruntQuickAttack(Puppet _puppet) : base(_puppet)
    {
        attackExtents = new Vector3(1, 0.5f, 3);
    }
            // La state machine de QuickAttack   :
    public override void OnBegin()
    {
        base.OnBegin();
        timer = 0;
        chargeTime = 0.1f;
        pushValue = 12;
        attackStarted = false;
        CurUpdateFct = Charge;
        puppet.Animator.SetTrigger("StartAttack");
        attackOrigin = puppet.transform.position + puppet.transform.forward * attackExtents.z;
    }
    public override void OnEnd()
    {
        base.OnEnd();
    }

    public void Charge()
    {
        puppet.Rb.velocity = Vector3.zero;
        timer += Time.deltaTime;
      

        if (timer> chargeTime)
        {
            timer = 0;
            CurUpdateFct = DuringAttack;
        }
    }
    public void DuringAttack()
    {

        puppet.Rb.velocity = puppet.stats.Get(Stats.StatType.move_speed)* puppet.transform.forward*1.5f;
        timer += Time.deltaTime;

        
        if (puppet.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
        {
            attackStarted = true;
        }
        if (attackStarted && puppet.Animator.IsInTransition(0))
        {
            timer = 0;
            attackStarted = false;
            CurUpdateFct = AttackEnd;
            puppet.Animator.SetFloat("Velocity", 0);
        }
    }
    public void AttackEnd()
    {
        puppet.Rb.velocity = Vector3.zero;
        timer += Time.deltaTime;
        if (timer > 0.2f)
        {
            CurUpdateFct = null;
            puppet.PuppetAction = new GruntAction(puppet); // On laisse la main
        }
    }

        // autres
   
    public override void OnAnimationEvent(string functionName)
    {
        puppet.GetComponentInChildren<ParticleSystem>().Play();
        //AttackCollision();
        puppet.AttackCollision(puppet,attackExtents, attackOrigin, pushValue);
    }

        // Les ordres/transition des brain   :

    public override void OnChargeAttack()
    {
        // interdiction de charger Une attaque
    }
    public override void SetVelocity(Vector3 velocity)
    {
        // interdiction de bouger
    }
    public override void SetRotation(Vector3 direction)
    {
        // interdiction de tourner
    }
    public override void OnSimpleAttack()
    {
        // Interdiction d'attaquer
    }
    // Gzimo debug
    Vector3 cubeCenter;
    public override void DrawGizmo()
    {
        base.DrawGizmo();
        GL.PushMatrix();
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(cubeCenter, puppet.transform.rotation, puppet.transform.lossyScale);
        Gizmos.matrix = rotationMatrix;
        Gizmos.color = new Color(0, 0, 1, 0.5f);
       // Gizmos.DrawCube(cubeCenter, attackExtents);
        Gizmos.DrawCube(Vector3.zero, attackExtents);
        //Gizmos.DrawCube()
        GL.PopMatrix();
    }
}
