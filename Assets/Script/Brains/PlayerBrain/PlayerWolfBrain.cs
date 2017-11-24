using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWolfBrain : PlayerBrain {

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Input.GetButton("Fire2"))
        {
            if (puppet.PuppetAction is WolfAction)
                (puppet.PuppetAction as WolfAction).JumpAttack();
        }
    }
}
