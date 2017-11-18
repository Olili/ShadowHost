using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntFollow_State : Follow_State
{
    public GruntFollow_State(Puppet _puppet) : base(_puppet)
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
    protected override void FollowMovingPlayer()
    {
        base.FollowMovingPlayer();

        Vector3 leaderPos = puppet.Leader.transform.position;
        Vector3 leaderVel = puppet.Leader.Rb.velocity;
        Vector3 leaderDir = puppet.Leader.transform.forward;


        Vector3 vecFromLeader = puppet.transform.position - leaderPos;

        if (vecFromLeader.magnitude < 1.5f)
        {
            //steering.Evade(leaderPos, leaderVel,2);
            steering.Flee(leaderPos, 2);
        }
        else
        {
            steering.Alignement(leaderDir, 0.3f);
            steering.Seek(leaderPos, 0.7f);
        }

        if (puppet.Leader.GetVelocity().magnitude < 0.2f)
        {
            FixedUpdateFct = FollowImmobilePlayer;
        }
        Move();
    }
    protected override void FollowImmobilePlayer()
    {
        base.FollowImmobilePlayer();

    }
    protected override void Move()
    {
        base.Move();

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

    /// <summary>
    /// Recherche l'ennemi de la horde identifié le plus proche de moi!
    /// </summary>
    protected override void CheckForFoes()
    {
        base.CheckForFoes();
    }


}
