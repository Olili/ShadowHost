using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseHumain_State : Chase_State
{
    float minDistAttack;
    float maxDistAttack;
    public ChaseHumain_State(Puppet _puppet) : base(_puppet)
    {
    }
    public override void OnBegin()
    {
        minDistAttack = 18;
        maxDistAttack = 25;
        base.OnBegin();
        FixedUpdateFct = GoToOpponent;
    }
    public override void OnEnd()
    {
        base.OnEnd();
    }
    public void GoToOpponent()
    {
        if (myTarget!=null && !(myTarget.PuppetAction is DeathAction))
        {
            Vector3 vecToTarget = myTarget.transform.position - puppet.transform.position;
            steering.Seek(myTarget.transform.position, 1);

            if (vecToTarget.magnitude < minDistAttack)
            {
                FixedUpdateFct = Attack;
            }

            Vector3 velocity = steering.ComputedVelocity;
            puppet.PuppetAction.SetVelocity(velocity);
            velocity.y = 0;
            puppet.PuppetAction.SetRotation(velocity.normalized);
        }
        else
        {
            if (!FindTheNearestFoe())
            {
                (puppet.brain as IA_Brain).MyIAState = (puppet.brain as IA_Brain).GetTypeState(Brain.E_State.follow);
            }
        }
    }
    public void Attack()
    {
        if (myTarget != null && !(myTarget.PuppetAction is DeathAction))
        {
            Vector3 vecToTarget = myTarget.transform.position - puppet.transform.position;
            float angleToTarget = Vector3.Angle(puppet.transform.forward, vecToTarget);

            if (puppet.PuppetAction is HumainAction)
                (puppet.PuppetAction as HumainAction).Charge();

            if (angleToTarget > 6)
            {
                puppet.PuppetAction.SetRotation(vecToTarget.normalized);
            }
            else
            {
                if (puppet.PuppetAction is HumainAction)
                    (puppet.PuppetAction as HumainAction).Shoot();
            }

            if (vecToTarget.magnitude > maxDistAttack)
            {
                FixedUpdateFct = GoToOpponent;
            }
        }
        else
        {
            if (!FindTheNearestFoe())
            {
                (puppet.brain as IA_Brain).MyIAState = (puppet.brain as IA_Brain).GetTypeState(Brain.E_State.follow);
            }
        }
    }

}
