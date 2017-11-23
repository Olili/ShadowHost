using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase_State : IA_State
{
    protected Puppet foeLeaderPuppet;
    protected Puppet myTarget = null;
    protected Vector3 myVel = Vector3.zero;

    public Chase_State(Puppet _puppet) : base(_puppet)
    {
    }

    public override void OnBegin()
    {
        foeLeaderPuppet = puppet.HordeManager.FoeLeaderPuppet;
        base.OnBegin();
    }
    public override void OnEnd()
    {
        base.OnEnd();
    }

    public virtual void FixedUpdate_ChaseFoe()
    {
        if (!FindTheNearestFoe())
        {
            (puppet.brain as IA_Brain).MyIAState = (puppet.brain as IA_Brain).GetTypeState(Brain.E_State.follow);
        }
    }
    public virtual void Update_ChaseFoe()
    {

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

    protected virtual bool FindTheNearestFoe()
    {
        float nearestDistance = float.MaxValue;
        Puppet myPossibleTarget = null;

        if (foeLeaderPuppet.HordeManager != null)
        {
            foreach (Puppet pup in foeLeaderPuppet.HordeManager.HordePuppets)
            {
                float tempDist = Vector3.Distance(puppet.transform.position, pup.transform.position);
                if (tempDist < nearestDistance)
                {
                    nearestDistance = tempDist;
                    myPossibleTarget = pup;
                }
            }
        }

        if (myPossibleTarget != null)
        {
            myTarget = myPossibleTarget;
            return true;
        }
        myTarget = null;
        return false;
    }
}
