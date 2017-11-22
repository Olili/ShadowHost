using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ProjectileType
{
    lance, maxProjectile
}
public class ProjectilePool : MonoBehaviour {
    [SerializeField] GameObject[] modelTab;
    List<GameObject>[] poolTab;
    int poolSize = 25;
    // Use this for initialization
    static private int ID = 0;
   

    void Start () {
        poolTab = new List<GameObject>[(int)ProjectileType.maxProjectile];
        for (int i = 0; i < (int)ProjectileType.maxProjectile; i++)
        {
            poolTab[i] = new List<GameObject>();
            for (int j = 0; j < poolSize; j++)
            {
            }
        }
        GameManager.Instance.ProjectilePool = this;
    }
    Projectile CreateProjectile(ProjectileType type)
    {
        GameObject poolObject = Instantiate<GameObject>(modelTab[(int)type]);
        poolObject.name = "lance" + (ID++);
        poolObject.transform.parent = transform;
        poolObject.SetActive(false);
        poolTab[(int)type].Add(poolObject);
        return poolObject.GetComponent<Projectile>();
    }
    public Projectile GetProjectile(ProjectileType type,Puppet _laucher,Vector3 position,Quaternion oriention)
    {
        List<GameObject> pool = poolTab[(int)type];
        Projectile returnedProjectile = null;
        for (int i = 0; i < pool.Count; i++)
        {
            Projectile projectile = pool[i].GetComponent<Projectile>();
            if (projectile == null) Debug.Log("error : fake prefab in pool");
            else
            {
                if (!projectile.gameObject.activeInHierarchy)
                {
                    returnedProjectile = projectile;
                    projectile.Init(_laucher);
                    break;
                }
            }
        }
        if (returnedProjectile == null)
        {
            returnedProjectile = CreateProjectile(type);
        }
        returnedProjectile.transform.position = position;
        returnedProjectile.transform.rotation = oriention;
        returnedProjectile.gameObject.SetActive(true);
        return returnedProjectile;
    }
    public void SendToPool(GameObject projectile)
    {
        projectile.transform.parent = transform;
        projectile.gameObject.SetActive(false);
    }



}
