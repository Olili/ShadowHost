using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAction : PuppetAction
{
    public WolfAction(Puppet _puppet) : base(_puppet)
    {
    }
  
    public override void SetVelocity(Vector3 velocity)
    {
        base.SetVelocity(velocity);
        puppet.Animator.SetFloat("Velocity", velocity.magnitude);
    }
    public virtual void JumpAttack()
    {
        puppet.PuppetAction = new WolfJumpAttack(puppet);
    }
}
