using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseSpider_State : Chase_State
{
    public ChaseSpider_State(Puppet _puppet) : base(_puppet)
    {

    }

    public override void OnBegin()
    {
        base.OnBegin();
        FixedUpdateFct = FixedUpdate_ChaseFoe;
        UpdateFct = Update_ChaseFoe;
        FindTheNearestFoe();
    }
    public override void OnEnd()
    {
        base.OnEnd();
    }

    public override void FixedUpdate_ChaseFoe()
    {
        base.FixedUpdate_ChaseFoe();

        if (myTarget != null && !(myTarget.PuppetAction is DeathAction))
        {
            myVel = puppet.Rb.velocity;

            Vector3 vecFromFoe = puppet.transform.position - myTarget.transform.position;

            if (vecFromFoe.magnitude < 2.0f)
            {
                puppet.transform.LookAt(new Vector3(myTarget.transform.position.x, puppet.transform.position.y, myTarget.transform.position.z));
                if (puppet.PuppetAction is SpiderAction)
                    (puppet.PuppetAction as SpiderAction).BasicAttack();
            }
            else
            {
                steering.Seek(myTarget.transform.position, 0.7f);
                Move();
            }

        }
        else
        {
            FindTheNearestFoe();
        }
    }
    public override void Update_ChaseFoe()
    {
        base.Update_ChaseFoe();
    }
    protected override void Move()
    {
        base.Move();

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

    protected override bool FindTheNearestFoe()
    {

        float nearestDistance = float.MaxValue;
        Puppet myPossibleTarget = null;

        if (foeLeaderPuppet != null)
        {
            if (foeLeaderPuppet.GetComponent<Alpha>() != null)
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
        }
        else
        {
            Debug.LogError("Pas de puppet leader ennemie!!");
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
