using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_State : IA_State {

    public bool chillTest;
    public Follow_State(Puppet _puppet) : base(_puppet)
    {

    }
    public override void OnBegin()
    {
        base.OnBegin();
        chillTest = false;
        FixedUpdateFct = FollowMovingPlayer;
    }
    public override void OnEnd()
    {
        base.OnEnd();
    }

    void FollowMovingPlayer() // fct state
    {
        Vector3 leaderPos = puppet.Leader.transform.position;
        Vector3 leaderVel = puppet.Leader.Rb.velocity;
        Vector3 leaderDir = puppet.Leader.transform.forward;

        Vector3 vecFromLeader = puppet.transform.position - leaderPos;

        if (vecFromLeader.magnitude < 2)
        {
            //steering.Evade(leaderPos, leaderVel,2);
            steering.Flee(leaderPos, 2);
        }
        else
        {
            steering.Alignement(leaderDir, 0.3f);
            steering.Seek(leaderPos, 0.7f);
        }

        if (puppet.Leader.GetVelocity().magnitude < 0.2f) // transition
        {
            FixedUpdateFct = FollowImmobilePlayer;
        }
        Move();
    }
    void FollowImmobilePlayer() // fct state
    {
        Vector3 leaderPos = puppet.Leader.transform.position;
        Vector3 leaderVel = puppet.Leader.Rb.velocity;
        Vector3 leaderDir = puppet.Leader.transform.forward;

        Vector3 vecFromLeader = puppet.transform.position - leaderPos;
        if (vecFromLeader.magnitude > (puppet.Extents.magnitude * 2))
        {
            if (!chillTest)
                steering.Arrival(leaderPos);
        }
        if (puppet.Leader.GetVelocity().magnitude > 0.2f) // transition
        {
            FixedUpdateFct = FollowMovingPlayer;
            chillTest = false;

        }
        Move();
    }
    void Move() // fct util pour toute state
    {
        Vector3 velocity = Vector3.zero;
        steering.Separation(0.7f);
        velocity = steering.ComputedVelocity;

        if (velocity.magnitude > 0.3f)
        {
            puppet.PuppetAction.SetVelocity(velocity);
            velocity.y = 0;
            puppet.PuppetAction.SetRotation(velocity.normalized);
        }
        else
        {
            puppet.PuppetAction.SetVelocity(Vector3.zero);
            puppet.PuppetAction.SetRotation(Vector3.zero);
        }
    }
}
