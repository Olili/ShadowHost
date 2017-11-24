using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHumanBrain : PlayerBrain
{
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Input.GetButtonDown("Fire1"))
        {
            if (puppet.PuppetAction is HumainAction)
                (puppet.PuppetAction as HumainAction).Charge();
        }
        if (!(Input.GetButton("Fire1")))
            if (puppet.PuppetAction is HumainAction)
                (puppet.PuppetAction as HumainAction).Shoot();
    }
    
}
