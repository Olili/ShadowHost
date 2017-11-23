using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_State : IA_State {

    public bool chillTest;

    protected float timerForCheckingFoes = 0.0f; // Permet d'éviter de faire des sphere-Cast à chaque frame
    protected float maxTimerForCheckingFoes = 1.0f; // check pour les ennemies toute les 1 sec;
    protected float radiusForCheckingFoes = 20.0f;

    public Follow_State(Puppet _puppet) : base(_puppet)
    {

    }
    public override void OnBegin()
    {
        base.OnBegin();
        timerForCheckingFoes = Random.value; // chaque puppet aura un timer différent => évite d'avoir tous les spherecast des puppet à la meme frame!
    }
    public override void OnEnd()
    {
        base.OnEnd();
    }

    protected virtual void FollowMovingPlayer()
    {

        CheckForFoes();
    }
    protected virtual void FollowImmobilePlayer()
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

        CheckForFoes();

    }
    protected virtual void Move()
    {
    }

    /// <summary>
    /// Recherche l'ennemi de la horde identifié le plus proche de moi!
    /// </summary>
    protected virtual void CheckForFoes()
    {
        timerForCheckingFoes += Time.deltaTime;
        if(timerForCheckingFoes >= maxTimerForCheckingFoes)
        {
            LayerMask possibleTarget = 1 << LayerMask.NameToLayer("Puppet");
            Collider[] allPossibleTarget = Physics.OverlapSphere(puppet.transform.position, radiusForCheckingFoes, possibleTarget, QueryTriggerInteraction.Ignore);

            for (int i = 0; i< allPossibleTarget.Length; i++)
            {
                Puppet spottedPuppet = allPossibleTarget[i].GetComponent<Puppet>();
                if (puppet.Leader != null)
                {
                 
                    IA_State tempLeaderState = puppet.Leader.gameObject.GetComponent<IA_Brain>().MyIAState;
                    //if (spottedPuppet.transform.parent != null && spottedPuppet.HordeManager != null)
                    {
                        if (spottedPuppet.Type != puppet.Type) // espece de creature différent de la mienne => combat de horde
                        {
                            // J'ai trouvé des ennemies!!!
                            if (tempLeaderState is Follow_State)
                            {
                                (tempLeaderState as Follow_State).OneOfMyPuppetFindFoes(spottedPuppet.Leader);
                            }
                        }
                        else if (spottedPuppet.Leader != puppet.Leader) // meme espece mais pas le meme leader => combat d'alpha
                        {
                            if (tempLeaderState is Follow_State)
                            {
                                if((spottedPuppet.HordeManager.HordePuppets.Count > 0))
                                {
                                    (tempLeaderState as Follow_State).AlphaFight(spottedPuppet.Leader);
                                }
                            }
                        }
                    }
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
    public virtual void OneOfMyPuppetFindFoes(Puppet _foePuppet)
    {
        puppet.HordeManager.FoeLeaderPuppet = _foePuppet;
        foreach (Puppet myFollowers in puppet.HordeManager.HordePuppets)
        {
            myFollowers.GetComponent<IA_Brain>().MyIAState = myFollowers.GetComponent<IA_Brain>().GetTypeState(Brain.E_State.chase);
        }
        //puppet.GetComponent<IA_Brain>().MyIAState = puppet.GetComponent<IA_Brain>().GetTypeState(Brain.E_State.chase);
    }
    public virtual void AlphaFight(Puppet _foePuppet)
    {
        puppet.HordeManager.FoeLeaderPuppet = _foePuppet;
        foreach (Puppet myFollowers in puppet.HordeManager.HordePuppets)
        {
            myFollowers.GetComponent<IA_Brain>().MyIAState = myFollowers.GetComponent<IA_Brain>().GetTypeState(Brain.E_State.alphaFight);
        }
        //puppet.GetComponent<IA_Brain>().MyIAState = puppet.GetComponent<IA_Brain>().GetTypeState(Brain.E_State.alphaFight);
    }

}

