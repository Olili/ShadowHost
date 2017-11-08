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

public class HordeManager : MonoBehaviour {

    CreaturePool creaturePool;

    void Start () {
        creaturePool = GetComponent<CreaturePool>();
	}
	
    void CreateHorde(Vector3 position,CreatureType type,int nbCreatures)
    {
        GameObject hordeContainer = new GameObject("horde");
        hordeContainer.transform.position = position;

        Puppet alpha = creaturePool.GetCreature(type);
        alpha.Init(position, null, hordeContainer.transform);
        alpha.gameObject.AddComponent<PlayerBrain>();
            // faire une carré de pop d'unités
        float margin = 1.3f;
        int rowZ = Mathf.FloorToInt(nbCreatures / 2);
        int rowX = Mathf.CeilToInt(nbCreatures / 2);

        Vector3 offset = position;
        offset.x -= ((rowX - 1) * margin / 2.0f);
        offset.z -= (margin);

        for (int z = 0; z < rowZ;z++)
        {
            for (int x = 0; x < rowX; x++)
            {
                Puppet follower = creaturePool.GetCreature(type);

                Vector3 creaturePosition = offset;
                creaturePosition.x += x * margin;
                creaturePosition.z -= z * margin;

                follower.Init(creaturePosition,alpha, hordeContainer.transform);
                follower.gameObject.AddComponent<FollowBrain>();
            }
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Salut");
            CreateHorde(Vector3.zero, CreatureType.Spider, 5);
        }
    }
}
