using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class Host : MonoBehaviour {


//    delegate void SimpleDelegate(Puppet dead);
//    SimpleDelegate OnDeadBodyClose;
//    Puppet closestDeadPuppet;
//    List<Puppet> deadBodyList;
//    Puppet puppet;
//    public void Start()
//    {
//        puppet = GetComponent<Puppet>();
//        deadBodyList = new List<Puppet>();
//    }
//    public void AddBody(Puppet deadPuppet)
//    {
//        if ((!deadBodyList.Contains(deadPuppet)))
//        {
//            deadBodyList.Add(deadPuppet);
//        }
//    }
//    public void RemoveBody(Puppet deadPuppet)
//    {
//        if ((deadBodyList.Contains(deadPuppet)))
//        {
//            deadBodyList.Remove(deadPuppet);
//        }
//    }
//    public void Update()
//    {
//        if (deadBodyList.Count != 0)
//        {
//            ClosestDeadBody();
//        }
//    }
//    public void ClosestDeadBody()
//    {
//        float closestDistance = 1000;
//        Puppet closest = null;
//        for (int i = 0; i < deadBodyList.Count; i++)
//        {
//            float distance = Vector3.Distance(deadBodyList[i].transform.position, puppet.transform.position);
//            if (distance < closestDistance)
//            {
//                closestDistance = distance;
//                closest = deadBodyList[i];
//            }
//        }
//        if (closest != closestDeadPuppet && OnDeadBodyClose != null)
//        {
//            closestDeadPuppet = closest;
//            OnDeadBodyClose(closestDeadPuppet);
//            Debug.Log("DeadBody = " + closest.gameObject.name);
//        }
//    }

//    public void OnDestroy()
//    {
//        OnDeadBodyClose -= GameManager.Instance.InterfaceManager.HighlightDeadPuppet;
//    }

//    public void AddBody(Puppet deadPuppet)
//    {
//        if ((!deadBodyList.Contains(deadPuppet)))
//        {
//            deadBodyList.Add(deadPuppet);
//        }
//    }
//    public void RemoveBody(Puppet deadPuppet)
//    {
//        if ((deadBodyList.Contains(deadPuppet)))
//        {
//            deadBodyList.Remove(deadPuppet);
//        }
//    }
//    public void Update()
//    {
//        if (deadBodyList.Count != 0)
//        {
//            ClosestDeadBody();
//        }
//    }
//    public void ClosestDeadBody()
//    {
//        float closestDistance = 1000;
//        Puppet closest = null;
//        for (int i = 0; i < deadBodyList.Count; i++)
//        {
//            float distance = Vector3.Distance(deadBodyList[i].transform.position, puppet.transform.position);
//            if (distance < closestDistance)
//            {
//                closestDistance = distance;
//                closest = deadBodyList[i];
//            }
//        }
//        if (closest != closestDeadPuppet && OnDeadBodyClose != null)
//        {
//            closestDeadPuppet = closest;
//            OnDeadBodyClose(closestDeadPuppet);
//            Debug.Log("DeadBody = " + closest.gameObject.name);
//        }
//    }
//}

public class Host : MonoBehaviour
{
    delegate void SimpleDelegate(Puppet dead);    SimpleDelegate OnDeadBodyClose;    Puppet closestDeadPuppet;    List<Puppet> deadBodyList;    Puppet puppet;
    public void Start()    {        puppet = GetComponent<Puppet>();        deadBodyList = new List<Puppet>();
    }    public void GoInBody(Puppet body)    {
        transform.parent = body.transform;        gameObject.SetActive(false);        GameManager.Instance.Possession(body);        (body.PuppetAction as DeathAction).secondLife = true;    }    public void GoOutBody(Puppet body)    {        transform.parent = null;        gameObject.SetActive(true);        body.PuppetAction = new DeathAction(body);
    }
    public void AddBody(Puppet deadPuppet)    {        if ((!deadBodyList.Contains(deadPuppet)))        {            deadBodyList.Add(deadPuppet);        }
    }    public void RemoveBody(Puppet deadPuppet)    {
        if ((deadBodyList.Contains(deadPuppet)))        {            deadBodyList.Remove(deadPuppet);            if (deadBodyList.Count == 0)                OnDeadBodyClose(null);        }    }    public void Update()
    {        if (deadBodyList.Count != 0)        {            ClosestDeadBody();        }        if (Input.GetButton("Fire1") && closestDeadPuppet != null)        {            GoInBody(closestDeadPuppet);        }    }    public void ClosestDeadBody()    {        float closestDistance = 1000;        Puppet closest = null;        for (int i = 0; i < deadBodyList.Count; i++)        {            float distance = Vector3.Distance(deadBodyList[i].transform.position, puppet.transform.position);            if (distance < closestDistance)            {                closestDistance = distance;                closest = deadBodyList[i];            }        }        if (closest != closestDeadPuppet)        {            closestDeadPuppet = closest;            Debug.Log("DeadBody = " + closest.gameObject.name);
            if (OnDeadBodyClose != null)
            {
                OnDeadBodyClose(closestDeadPuppet);
            }                   }
    }
}