using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntAction : PuppetAction
{

    public GruntAction(Puppet _puppet) : base(_puppet)
    {

    }
    public override void BasicAttack()
    {
        OnSimpleAttack();
    }

    public virtual void OnSimpleAttack()
    {
        puppet.PuppetAction = new GruntQuickAttack(puppet);
    }
    public virtual void OnChargeAttack()
    {

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
