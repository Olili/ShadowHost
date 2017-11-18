using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_State : IA_State {

    public bool chillTest;

    private float timerForCheckingFoes = 0.0f; // Permet d'éviter de faire des sphere-Cast à chaque frame
    private float maxTimerForCheckingFoes = 1.0f; // check pour les ennemies toute les 1 sec;
    private float radiusForCheckingFoes = 20.0f;

    public Follow_State(Puppet _puppet) : base(_puppet)
    {

    }
    public override void OnBegin()
    {
        base.OnBegin();
        chillTest = false;
        FixedUpdateFct = FollowMovingPlayer;
        timerForCheckingFoes = Random.value; // chaque puppet aura un timer différent => évite d'avoir tous les spherecast des puppet à la meme frame!
    }
    public override void OnEnd()
    {
        base.OnEnd();
    }

    void FollowMovingPlayer()
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

        if (puppet.Leader.GetVelocity().magnitude < 0.2f)
        {
            FixedUpdateFct = FollowImmobilePlayer;
        }
        Move();
    }
    void FollowImmobilePlayer()
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
    void Move()
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

    /// <summary>
    /// Recherche l'ennemi de la horde identifié le plus proche de moi!
    /// </summary>
    void CheckForFoes()
    {
        timerForCheckingFoes += Time.deltaTime;
        if(timerForCheckingFoes >= maxTimerForCheckingFoes)
        {
            LayerMask possibleTarget = 1 << LayerMask.NameToLayer("Puppet");
            Collider[] allPossibleTarget = Physics.OverlapSphere(puppet.transform.position, radiusForCheckingFoes, possibleTarget, QueryTriggerInteraction.Ignore);

            for (int i = 0; i< allPossibleTarget.Length; i++)
            {
                if (allPossibleTarget[i].GetComponent<Puppet>().GetType() != puppet.GetType()) // espece de creature différent de la mienne => combat de horde
                {
                    // J'ai trouvé des ennemies!!!
                    IA_State tempLeaderState = puppet.Leader.gameObject.GetComponent<IA_Brain>().MyIAState;
                    if(tempLeaderState is Follow_State)
                    {
                        (tempLeaderState as Follow_State).OneOfMyPuppetFindFoes(allPossibleTarget[i].GetComponent<Puppet>());
                    }
                }
                else if (allPossibleTarget[i].GetComponent<Puppet>().Leader != puppet.Leader) // meme espece mais pas le meme leader => combat d'alpha state
                {

                }
            }
            timerForCheckingFoes = 0.0f;
        }
    }

    /// <summary>
    /// Fonction qui permet à une puppet de prevenir l'aplha qu'elle a vu un ennemie.
    /// Fonction appelé uniquement chez l'alpha.
    /// </summary>
    /// <param name="_FoePuppet"> puppet ennemie detecté </param>
    public void OneOfMyPuppetFindFoes(Puppet _foePuppet)
    {
        puppet.HordeManager.FoeLeaderPuppet = _foePuppet;
        foreach (Puppet myFollowers in puppet.GetComponent<Alpha>().HordePuppets)
        {
            myFollowers.GetComponent<IA_Brain>().MyIAState = new Chase_State(puppet);
        }
        puppet.GetComponent<IA_Brain>().MyIAState = new Chase_State(puppet);
    }
}

