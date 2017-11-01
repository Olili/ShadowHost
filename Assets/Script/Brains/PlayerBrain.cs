﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrain : Brain {

    Rigidbody rb;
    StatBuff turnSpeedBuff;
    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();

    }
    public override void Start () {
        base.Start();
        turnSpeedBuff = new StatBuff(Stats.StatType.maxTurnSpeed, 500, -1);
        puppet.stats.AddBuff(turnSpeedBuff);
	}
    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate () {

        base.FixedUpdate();
        Vector3 direction= Vector3.zero;
        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");
        direction.Normalize();
        direction = direction* puppet.stats.Get(Stats.StatType.move_speed);
        direction.y = rb.velocity.y;

        puppet.SetVelocity(direction);
        puppet.Rotate(direction.normalized);
    }
    public override void OnDestroy()
    {
        puppet.stats.RemoveBuff(turnSpeedBuff);
    }
}
