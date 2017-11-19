using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Responsable de l'apparition d'une horde dans le monde. 
*/
public enum CreatureType
{
    Spider,Grunt, Max_Creatures
}

public class HordeCreator : MonoBehaviour {

    CreaturePool creaturePool;
    List<HordeManager> hordeList;
    List<Puppet> deadList;
    static readonly float maxDead = 20;
    float timer;
    public float hordePopDelay = 5;
    public int nbCreaturePop = 10;
    public int maxHordeDistance = 15;

    private void Awake()
    {
        GameManager.Instance.hordeCreator = this;
        hordeList = new List<HordeManager>();
        timer = 0;
    }
    void Start () {
        creaturePool = GetComponent<CreaturePool>();
        deadList = new List<Puppet>(nbCreaturePop);
    }
	
    public void CreateHorde(Vector3 position,CreatureType type,int nbCreatures)
    {
        GameObject hordeContainer = new GameObject("horde");
        hordeContainer.AddComponent<HordeManager>();
        hordeContainer.transform.position = position;

        Puppet alpha = creaturePool.GetCreature(type);
        alpha.name = "Alpha_" + type.ToString();
        hordeContainer.GetComponent<HordeManager>().InitAlpha(alpha);
        alpha.Init(position, alpha, hordeContainer.transform);
        alpha.gameObject.AddComponent<Alpha>();
        alpha.GetComponent<Alpha>().Init();
        alpha.gameObject.AddComponent<IA_Brain>();
        alpha.ChangeColorDebug();

        if (hordeList == null)
            hordeList = new List<HordeManager>();
        hordeList.Add(hordeContainer.GetComponent<HordeManager>());

            // faire une carré de pop d'unités
        float margin = 1.3f;
        int rowZ = Mathf.CeilToInt(Mathf.Sqrt(nbCreatures));
        int rowX = Mathf.CeilToInt(Mathf.Sqrt(nbCreatures));


        Vector3 offset = position;
        offset.x -= ((rowX - 1) * margin / 2.0f);
        offset.z -= (margin);

        int nbUnit = 0;
        for (int z = 0; z < rowZ;z++)
        {
            for (int x = 0; x < rowX; x++)
            {
                Puppet follower = creaturePool.GetCreature(type);

                Vector3 creaturePosition = offset;
                creaturePosition.x += x * margin;
                creaturePosition.z -= z * margin;

                follower.Init(creaturePosition,alpha, hordeContainer.transform);
                follower.gameObject.AddComponent<IA_Brain>();
                alpha.transform.parent.GetComponent<HordeManager>().AddHordePuppet(follower);
                if (++nbUnit >= nbCreatures)
                    return;
            }
        }
    }
    public void DestroyHorde(HordeManager hordeManager)
    {
        if (hordeList.Contains(hordeManager))
            hordeList.Remove(hordeManager);
        if (hordeManager.HordePuppets!=null)
        {
            for (int i = 0; i < hordeManager.HordePuppets.Count; i++)
            {
                SendtoPool(hordeManager.HordePuppets[i]);
                if (hordeManager.HordePuppets[i].GetComponent<Alpha>())
                    Destroy(hordeManager.HordePuppets[i].GetComponent<Alpha>());
            }
        }
        Destroy(hordeManager.gameObject);
    }
    public void SendtoPool(Puppet puppet)
    {
        puppet.gameObject.SetActive(false);
        puppet.transform.parent = transform;
        if (puppet.GetComponent<Brain>() != null)
            Destroy(puppet.GetComponent<Brain>());
    }
    public void CreateDeadPuppet(CreatureType type, Vector3? position = null)
    {
        Puppet puppet = creaturePool.GetCreature(type);
        puppet.PuppetAction = new DeathAction(puppet);
        if(position.HasValue)
        {
            puppet.transform.position = position.Value;
        }
    }
    public void AddDeadPuppet(Puppet puppet)
    {
        if (deadList.Contains(puppet))
            return;
        deadList.Add(puppet);
        if (deadList.Count > maxDead)
        {
            SendtoPool(deadList[0]);
            deadList.Remove(deadList[0]);
        }
    }
    public void RemoveDeadPuppet(Puppet puppet)
    {
        if (deadList.Contains(puppet))
        {
            deadList.Remove(puppet);
        }
    }
    void Update()
    {
        PopHordeContinue();
    }


    public void PopHordeContinue()
    {
        // si le player est controller de horde
        PlayerBrain playerBrain = GameManager.Instance.PlayerBrain;
        RemoveFarAwayHorde(playerBrain);

        // si le player est ne mode hôte ou n'a pas de horde
        if (playerBrain.GetComponent<Host>() != null)
            LonelyPlayerHordePop(playerBrain);
        else
        {
            HordeManager playerHordeManager = playerBrain.transform.parent.GetComponent<HordeManager>();
            if (playerHordeManager!=null)
            {
                if (playerHordeManager.HordePuppets.Count < 1) 
                    LonelyPlayerHordePop(playerBrain);
                else
                    PlayerLeaderPopHorde(playerBrain);
            }
        }
    }
    public void LonelyPlayerHordePop(PlayerBrain playerBrain)
    {
        if (hordeList.Count > 3)
        {
        }
        else
        {
            timer += Time.deltaTime;

            if (timer > hordePopDelay)
            {
                timer = 0;
                PopHordeAroundPlayer(playerBrain);
            }
        }
    }

    public void PlayerLeaderPopHorde(PlayerBrain playerBrain)
    {
        if (hordeList.Count > 2 )
        {
            return;
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > hordePopDelay)
            {
                timer = 0;
                PopHordeAroundPlayer(playerBrain);
            }
        }
    }
    public void PopHordeAroundPlayer(PlayerBrain playerBrain)
    {
        Vector3 direction = Quaternion.AngleAxis(Random.Range(0, 180), Vector3.up) * playerBrain.transform.forward;
        Vector3 position = playerBrain.transform.position + direction * 10;
        CreateHorde(position, (CreatureType)Random.Range(0, (int)CreatureType.Max_Creatures), 10);
    }
    public void RemoveFarAwayHorde(PlayerBrain playerBrain)
    {
        for (int i = 0; i < hordeList.Count; i++)
        {
            float distance = Vector3.Distance(hordeList[i].CurrentAlpha.transform.position, playerBrain.transform.position);
            if (distance > maxHordeDistance)
            {
                DestroyHorde(hordeList[i]);
            }
        }
    }

}


//spawn rules : Stupid simple iteration : 
/*
 *  * Pas de systeme de zone
 *  Si le joueur est chef de horde :  Ne fait pop qu'une horde à la fois. 
 *      --> Faire pop une horde proche qui va vers lui allié / ennemis
 *  Si le joueur n'est pas chef de horde. : 2 hordes  
 *   --> Faire pop une horde proche qui va vers lui
 *      ---> Si elle est suffisamment loin du joueur disparait.
 *      ---> Si le joueur est suffisamment proche on pop une horde proche de lui
*/
