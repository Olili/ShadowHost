using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowBrain : Brain
{

    Steering steering;
    public bool chillTest = false;
    public override void Awake()
    {
        base.Awake();
        steering = GetComponent<Steering>();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Vector3 leaderPos = puppet.Leader.transform.position;
        Vector3 leaderVel = puppet.Leader.GetVelocity();
        Vector3 leaderDir = puppet.Leader.transform.forward;
     
        Vector3 velocity = Vector3.zero;

        // qu'est-ce que le leader Following ? 
        // il faut chercher à rattraper le joueur. 

        // On peut s'aligner dans la direction du joueur pour simplifier les déplacements.
        // On doit éviter le joueur s'il avance.
        if (puppet.Leader.GetVelocity().magnitude < 0.2f)
        {
            FollowImmobilePlayer(leaderPos, leaderVel, leaderDir);

        }
        else
        {
            chillTest = false;
            FollowMovingPlayer(leaderPos, leaderVel, leaderDir);
        }

        steering.Separation(0.7f);

        velocity = steering.ComputedVelocity;
        velocity.y = 0;
        if (velocity.magnitude > 0.3f)
        {
            puppet.PuppetAction.SetVelocity(velocity);
            puppet.PuppetAction.SetRotation(velocity.normalized);
        }
        else
        {
            puppet.PuppetAction.SetVelocity(Vector3.zero);
            puppet.PuppetAction.SetRotation(Vector3.zero);
        }
    }
    void FollowMovingPlayer(Vector3 leaderPos,Vector3 leaderVel,Vector3 leaderDir)
    {
        Vector3 vecFromLeader = transform.position - leaderPos;

       
        if (vecFromLeader.magnitude < 2 )
        {
            //steering.Evade(leaderPos, leaderVel,2);
            steering.Flee(leaderPos,2);
        }
        else
        {
            steering.Alignement(leaderDir, 0.3f);
            steering.Seek(leaderPos, 0.7f);
        }
    }
    void FollowImmobilePlayer(Vector3 leaderPos, Vector3 leaderVel, Vector3 leaderDir)
    {
        Vector3 vecFromLeader = transform.position - leaderPos;
        if (vecFromLeader.magnitude > (puppet.Extents.magnitude * 2))
        {
            if (!chillTest)
                steering.Arrival(leaderPos);
        }
    }
}
