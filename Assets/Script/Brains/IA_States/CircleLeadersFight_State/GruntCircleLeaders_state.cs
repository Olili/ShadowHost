using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntCircleLeaders_state : CircleLeaders_state
{
    public GruntCircleLeaders_state(Puppet _puppet) : base(_puppet)
    {
    }
    public override void OnBegin()
    {
        base.OnBegin();
        speed = 1.0f;
    }
    public override void OnEnd()
    {
        base.OnEnd();
    }
    protected override void Rotate()
    {
        Vector3 myVel = puppet.Rb.velocity;

        float deltaDistanceFromFight = Vector3.Distance(puppet.transform.position, puppet.Leader.transform.position) - distanceFromFight;
        Vector3 tempDirection = Vector3.Normalize(Vector3.Cross(puppet.transform.position - puppet.Leader.transform.position, Vector3.up));

        Vector3 direction = (tempDirection * 3.0f) + (deltaDistanceFromFight * Vector3.Normalize(puppet.transform.position - puppet.Leader.transform.position) * 0.1f);

        steering.Seek(direction, 0.7f);
        Move();
    }
}
