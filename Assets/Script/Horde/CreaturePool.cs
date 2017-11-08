using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Pool de creatures. 
Contient des listes de crétures initialisées. Prêtes à être apparraitre en jeux. 
Il faut gérer plusieurs creatures différentes. 
 */


public class CreaturePool : MonoBehaviour
{
    List<GameObject>[] poolTab; // contient un tableau de pool de creatures
    int poolSize;
    [SerializeField] GameObject[] prefabModel;

    private void Awake()
    {
        CreatePools();
    }

    void CreatePools()
    {
        poolTab = new List <GameObject>[(int)CreatureType.Max_Creatures];
        for (int i = 0; i < (int)CreatureType.Max_Creatures; i++)
        {
            poolTab[i] = new List<GameObject>();
            // remplir les pool de creatures au debut du jeux
        }
    }
    Puppet AddCreatureToPool(CreatureType type)
    {
        GameObject poolObject = Instantiate<GameObject>(prefabModel[(int)type]);
        poolTab[(int)type].Add(poolObject);
        Puppet puppet = poolObject.GetComponent<Puppet>();
        return puppet;
    }
    public Puppet GetCreature(CreatureType type)
    {
        List<GameObject> pool = poolTab[(int)type];
        Puppet returnedPuppet = null;
        for (int i = 0; i < pool.Count; i++)
        {
            Puppet puppet = pool[i].GetComponent<Puppet>();
            if (puppet == null) Debug.Log("error : fake prefab in pool");
            else
            {
                if (!puppet.gameObject.activeInHierarchy)
                {
                    return puppet;
                }
            }
        }
        if (returnedPuppet == null)
        {
            returnedPuppet = AddCreatureToPool(type);
        }
        return returnedPuppet;
    }
}
