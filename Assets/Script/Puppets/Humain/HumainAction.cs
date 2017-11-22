using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HumainAction : PuppetAction
{
    public HumainAction(Puppet _puppet) : base(_puppet)
    {
    }
    public virtual void Charge()
    {
        puppet.PuppetAction = new HumainChargedShoot(puppet);
    }
    public virtual void Shoot()
    {

    }
    public virtual  void RetreatAttack()
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
