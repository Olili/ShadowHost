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
              
                if (spottedPuppet.Type != puppet.Type) // espece de creature différent de la mienne => combat de horde
                {
                    puppet.HordeManager.PuppetFindFoe(spottedPuppet.Leader);
                }
                else if (spottedPuppet.Leader != puppet.Leader) // meme espece mais pas le meme leader => combat d'alpha
                {
                    puppet.HordeManager.StartAlphaFight(spottedPuppet.Leader);
                }
            }
            timerForCheckingFoes = 0.0f;
        }
    }
}

