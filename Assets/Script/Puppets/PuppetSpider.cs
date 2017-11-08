using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetSpider : Puppet {

    public override void SetVelocity(Vector3 velocity)
    {
        base.SetVelocity(velocity);
        if (animator!=null)
        {
            animator.SetFloat("Velocity", velocity.magnitude);
        }
    }
}
