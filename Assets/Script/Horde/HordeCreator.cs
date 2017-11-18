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
    List<Puppet> deadList;
    static readonly float maxDead = 20;


    public int nbCreaturePop = 10;

    private void Awake()
    {
        GameManager.Instance.hordeCreator = this;
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
        alpha.name = "AlphaSpider";
        hordeContainer.GetComponent<HordeManager>().InitAlpha(alpha);
        alpha.Init(position, alpha, hordeContainer.transform);
        alpha.gameObject.AddComponent<Alpha>();
        alpha.gameObject.AddComponent<IA_Brain>();
        alpha.ChangeColorDebug();
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
    public void SendtoPool(Puppet puppet)
    {
        puppet.gameObject.SetActive(false);
        puppet.transform.parent = transform;
    }
<<<<<<< Updated upstream
    public void CreateDeadPuppet(CreatureType type, Vector3? position = null)
    {
        Puppet puppet = creaturePool.GetCreature(type);
        puppet.PuppetAction = new DeathAction(puppet);
        if(position.HasValue)
        {
            puppet.transform.position = position.Value;
        }
=======
    public void CreateDeadPuppet(CreatureType type)
    {
        Puppet puppet = creaturePool.GetCreature(type);
        puppet.PuppetAction = new DeathAction(puppet);
>>>>>>> Stashed changes
    }
    public void AddDeadPuppet(Puppet puppet)
    {
        deadList.Add(puppet);
        if (deadList.Count > maxDead)
        {
            SendtoPool(deadList[0]);
            deadList.Remove(deadList[0]);
        }
    }
    void Update()
    {
      
    }
}
