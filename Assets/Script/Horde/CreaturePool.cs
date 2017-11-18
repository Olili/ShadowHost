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
    int poolSize = 25;
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
            for (int j = 0; j < poolSize; j++)
            {
                CreateAndAddToPool(CreatureType.Spider);
            }
        }
    }
    Puppet CreateAndAddToPool(CreatureType type)
    {
        GameObject poolObject = Instantiate<GameObject>(prefabModel[(int)type]);
        poolObject.transform.parent = transform;
        poolObject.SetActive(false);
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
                    returnedPuppet = puppet;
                    break;
                }
            }
        }
        if (returnedPuppet == null)
        {
            returnedPuppet = CreateAndAddToPool(type);
        }
        returnedPuppet.gameObject.SetActive(true);
        return returnedPuppet;
    }
}
