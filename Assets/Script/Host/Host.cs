using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Host : MonoBehaviour 
{
    delegate void SimpleDelegate(Puppet dead);
    SimpleDelegate OnDeadBodyClose;
    Puppet closestDeadPuppet;
    List<Puppet> deadBodyList;
    Puppet puppet;
    public void Start()
    {
        puppet = GetComponent<Puppet>();
        deadBodyList = new List<Puppet>();
        // OnDeadBodyClose += GameManager.Instance.interfaceManager.
    }
    public void AddBody(Puppet deadPuppet)
    {
        if ((!deadBodyList.Contains(deadPuppet)))
        {
            deadBodyList.Add(deadPuppet);
        }
    }
    public void RemoveBody(Puppet deadPuppet)
    {
        if ((deadBodyList.Contains(deadPuppet)))
        {
            deadBodyList.Remove(deadPuppet);
        }
    }
    public void Update()
    {
        if (deadBodyList.Count != 0)
        {
            ClosestDeadBody();
        }
    }
    public void ClosestDeadBody()
    {
        float closestDistance = 1000;
        Puppet closest = null;
        for (int i = 0; i < deadBodyList.Count; i++)
        {
            float distance = Vector3.Distance(deadBodyList[i].transform.position, puppet.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = deadBodyList[i];
            }
        }
        if (closest != closestDeadPuppet && OnDeadBodyClose != null)
        {
            closestDeadPuppet = closest;
            OnDeadBodyClose(closestDeadPuppet);
            Debug.Log("DeadBody = " + closest.gameObject.name);
        }
    }
}
