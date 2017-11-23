using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAlphasFight_State : AlphasFight_State
{

    public SpiderAlphasFight_State(Puppet _puppet) : base(_puppet)
    {
    }
    public override void OnBegin()
    {
        base.OnBegin();
        alphaOpposent = puppet.HordeManager.FoeLeaderPuppet;
        FixedUpdateFct = GoToMyOpponent;
    }
    public override void OnEnd()
    {
        base.OnEnd();
    }

    public override void GoToMyOpponent()
    {
        base.GoToMyOpponent();
        if (alphaOpposent != null && !(alphaOpposent.PuppetAction is DeathAction))
        {
            Vector3 myVel = puppet.Rb.velocity;

            Vector3 vecFromFoe = puppet.transform.position - alphaOpposent.transform.position;

            if (vecFromFoe.magnitude < 2.0f)
            {
                puppet.transform.LookAt(new Vector3(alphaOpposent.transform.position.x, puppet.transform.position.y, alphaOpposent.transform.position.z));

            }
            else
            {
                steering.Seek(alphaOpposent.transform.position, 0.7f);
                Move();
            }

        }

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
    public override void Fight()
    {
        base.Fight();
    }
}
