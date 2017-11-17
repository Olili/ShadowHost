using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerGruntBrain : PlayerBrain
{
    public override void Start()
    {
        base.Start();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Input.GetButton("Fire2"))
        {
            (puppet.PuppetAction as GruntAction).OnSimpleAttack();
        }
    }
}