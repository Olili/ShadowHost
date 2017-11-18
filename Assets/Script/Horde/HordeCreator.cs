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
    public int nbCreaturePop = 10;

    void Start () {
        creaturePool = GetComponent<CreaturePool>();
	}
	
    public void CreateHorde(Vector3 position,CreatureType type,int nbCreatures)
    {
        GameObject hordeContainer = new GameObject("horde");
        hordeContainer.AddComponent<HordeManager>();
        hordeContainer.transform.position = position;

        Puppet alpha = creaturePool.GetCreature(type);
        alpha.name = "AlphaSpider";
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
                alpha.GetComponent<Alpha>().AddHordePuppet(follower);
                if (++nbUnit >= nbCreatures)
                    return;
            }
        }
    }
    public void CreateDeadPuppet(CreatureType type)
    {
        Puppet puppet = creaturePool.GetCreature(type);
        puppet.PuppetAction = new DeathAction(puppet);
    }

    void Update()
    {
      
    }
}
