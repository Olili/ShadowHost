using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAction : PuppetAction
{
    public SpiderAction(Puppet _puppet) : base(_puppet)
    {
    }

    public override void OnBegin()
    {
        base.OnBegin();
    }

    public override void OnEnd()
    {
        base.OnEnd();
    }

    public override void SetRotation(Vector3 direction)
    {
        base.SetRotation(direction);
    }

    public override void SetVelocity(Vector3 velocity)
    {
        base.SetVelocity(velocity);
        if (puppet.Animator != null)
        {
            puppet.Animator.SetFloat("Velocity", velocity.magnitude);
        }
    }
}
