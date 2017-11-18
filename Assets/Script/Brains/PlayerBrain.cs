using System.Collections;
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
        puppet.Leader = puppet;
        GameManager.Instance.PlayerBrain = this;
    }
    public override void Update()
    {
        base.Update();
    }

    public virtual void Move()
    {
        Vector3 direction = Vector3.zero;
        direction += Camera.main.transform.forward * Input.GetAxis("Vertical");
        direction += Camera.main.transform.right * Input.GetAxis("Horizontal");
        direction.y = 0;
        direction.Normalize();
        direction = direction * puppet.stats.Get(Stats.StatType.move_speed);
        if (direction!=Vector3.zero)
            direction.y = 0;

        direction.y = puppet.Rb.velocity.y;

        if (puppet.IsOnGround == true)
            puppet.PuppetAction.SetVelocity(direction);


        direction = GetComponent<Steering>().GetDvOnPlan(transform.position + direction);
        puppet.PuppetAction.SetRotation(direction.normalized);
    }

    public override void FixedUpdate () {

        base.FixedUpdate();
        Move();
    }
    public override void OnDestroy()
    {
        puppet.stats.RemoveBuff(turnSpeedBuff);
    }
}
