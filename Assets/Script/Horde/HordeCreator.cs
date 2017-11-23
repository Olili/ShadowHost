using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Responsable de l'apparition d'une horde dans le monde. 
*/
public enum CreatureType
{
    Spider,Grunt,Humain,Wolf, Max_Creatures
}

public class HordeCreator : MonoBehaviour {

    CreaturePool creaturePool;
    [SerializeField] List<HordeManager> hordeList;
    List<Puppet> deadList;
    static readonly float maxDead = 20;
    float timer;
    public float hordePopDelay = 5;
    public int nbCreaturePop = 10;
    public int maxHordeDistance = 30;
    public int minHordeDistance = 15;
    public bool canGenerateHorde = true;
    [SerializeField] private int maxHordeInGame = 1;
    [SerializeField] private int ID;


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
            // Creation hordeManager
        GameObject hordeContainer = new GameObject("horde"+ type+" "+ (ID++));
        HordeManager hordeManager = hordeContainer.AddComponent<HordeManager>();
        hordeContainer.transform.position = position;

            // création puppet qui sera alpha
        Puppet alphaPuppet = creaturePool.GetCreature(type);
        hordeManager.InitAlpha(alphaPuppet);

        alphaPuppet.name = "Alpha_" + type.ToString()+" "+ (ID++);

            // ajout brain
        alphaPuppet.gameObject.AddComponent<IA_Brain>();
            // init alpha Puppet
        alphaPuppet.Init(position, alphaPuppet, hordeContainer.transform, hordeManager);
        

        if (hordeList == null)
            hordeList = new List<HordeManager>();
        hordeList.Add(hordeManager);

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

                follower.gameObject.AddComponent<IA_Brain>();
                follower.Init(creaturePosition,alphaPuppet, hordeContainer.transform, hordeManager);
                if (++nbUnit >= nbCreatures)
                    return;
            }
        }
    }
    public void DestroyHorde(HordeManager hordeManager)
    {
        if (hordeManager.HordePuppets != null)
        {
            for (int i = 0; i < hordeManager.HordePuppets.Count; i++)
            {
                Brain brain = hordeManager.HordePuppets[i].brain;
                if (brain != null)
                    Destroy(brain);
                SendtoPool(hordeManager.HordePuppets[i]);
            }
        }
        if (hordeList.Contains(hordeManager))
            hordeList.Remove(hordeManager);
        Destroy(hordeManager.gameObject);
    }
    void SendtoPool(Puppet puppet)
    {
        puppet.gameObject.SetActive(false);
        puppet.transform.parent = transform;
        
    }
    public void CreateDeadPuppet(CreatureType type, Vector3? position = null)
    {
        Puppet puppet = creaturePool.GetCreature(type);
        puppet.PuppetAction = new DeathAction(puppet);
        if(position.HasValue)
        {
            puppet.transform.position = position.Value;
        }
        AddDeadPuppet(puppet);
    }
    public void AddDeadPuppet(Puppet puppet)
    {
        if (deadList.Contains(puppet))
            return;
        deadList.Add(puppet);

        puppet.transform.SetParent(transform,true);
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
        if (canGenerateHorde)
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
            HordeManager playerHordeManager = playerBrain.puppet.HordeManager;
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
        if (hordeList.Count >= maxHordeInGame) // le player est une horde ou pas ?
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

    public void PlayerLeaderPopHorde(PlayerBrain playerBrain)
    {
        if (hordeList.Count >= maxHordeInGame)
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


        Vector3 direction = (Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * Vector3.forward).normalized;
        Vector3 position = playerBrain.transform.position + (direction * minHordeDistance);
        RaycastHit hit;
        if (Physics.Raycast(position + (Vector3.up*100),Vector3.down,out hit,200))
        {
            CreateHorde(hit.point, (CreatureType)Random.Range(0, creaturePool.prefabModel.Length), Random.Range(5, 12));
        }
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
