using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase_State : IA_State
{
    protected Puppet foeLeaderPuppet;
    protected Puppet myTarget = null;
    protected Vector3 myVel = Vector3.zero;

    protected float timerForCheckingFoes = 0.0f; // Permet d'éviter de faire des sphere-Cast à chaque frame
    protected float maxTimerForCheckingFoes = 1.0f; // check pour les ennemies toute les 1 sec;
    protected float radiusForCheckingFoes = 6.00f;
    protected float radiusForFight = 4.00f;


    public Chase_State(Puppet _puppet) : base(_puppet)
    {
    }

    public override void OnBegin()
    {
        foeLeaderPuppet = puppet.HordeManager.FoeLeaderPuppet;
        FixedUpdateFct = FixedUpdate_ChaseFoe;
        base.OnBegin();
    }
    public override void OnEnd()
    {
        base.OnEnd();
    }

    public virtual void FixedUpdate_ChaseFoe()
    {
        if (!CheckTarget())
        {
            (puppet.brain as IA_Brain).MyIAState = (puppet.brain as IA_Brain).GetTypeState(Brain.E_State.follow);
            return;
        }

        FindTheNearestFoe();
      
        if (myTarget!=null)
        {
            if (Vector3.Distance(puppet.transform.position, myTarget.transform.position) > radiusForFight)
            {
                GoToTarget();
                Move();
            }
            else
            {
                Brain brain = puppet.brain;
                (brain as IA_Brain).MyIAState = (brain as IA_Brain).GetTypeState(Brain.E_State.fight);
                ((brain as IA_Brain).MyIAState as Fight_State).myTarget = myTarget;
            }
        }
    }
    public virtual bool CheckTarget()
    {
        if (myTarget==null|| (myTarget.PuppetAction is DeathAction))
        {
            myTarget = puppet.HordeManager.GetTarget();
        }
        return myTarget != null;
    }

    protected virtual bool FindTheNearestFoe()
    {
        timerForCheckingFoes += Time.deltaTime;
        if (timerForCheckingFoes >= maxTimerForCheckingFoes)
        {
            LayerMask possibleTarget = 1 << LayerMask.NameToLayer("Puppet");
            Collider[] allPossibleTarget = Physics.OverlapSphere(puppet.transform.position, radiusForCheckingFoes, possibleTarget, QueryTriggerInteraction.Ignore);
            float nearestDistance = float.MaxValue;

            for (int i = 0; i < allPossibleTarget.Length; i++)
            {
                Puppet spottedPuppet = allPossibleTarget[i].GetComponent<Puppet>();
                if (spottedPuppet.Type != puppet.Type)
                {
                    float tempDist = Vector3.Distance(puppet.transform.position, spottedPuppet.transform.position);
                    if (tempDist < nearestDistance)
                    {
                        nearestDistance = tempDist;
                        myTarget = spottedPuppet;
                    }
                }

            }
            timerForCheckingFoes = 0.0f;
        }

            // transition 
      
        return true;
    }


    public virtual void GoToTarget()
    {
        steering.Seek(myTarget.transform.position);
    }
    protected virtual void Move()
    {
        Vector3 velocity = Vector3.zero;
        steering.Separation(0.7f);
        velocity = steering.ComputedVelocity;

        if (velocity.magnitude > 0.3f)
        {
            puppet.PuppetAction.SetVelocity(velocity);
            velocity.y = 0;
            puppet.PuppetAction.SetRotation(velocity.normalized);
        }
        else
        {
            puppet.PuppetAction.SetVelocity(Vector3.zero);
            puppet.PuppetAction.SetRotation(Vector3.zero);
        }
    }
}
