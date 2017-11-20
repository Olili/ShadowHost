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
        OnDeadBodyClose += GameManager.Instance.FeedbackManager.HighlightDeadPuppet;
    }

    public void OnDisable()
    {
        if(OnDeadBodyClose != null)
        {
            closestDeadPuppet = null;
            deadBodyList.Clear();
            OnDeadBodyClose(null);
        }
        // OnDeadBodyClose -= GameManager.Instance.FeedbackManager.HighlightDeadPuppet;
    }
    public void GoInBody(Puppet body)
    {
        //if (body.PuppetAction is Dea)
        if (body.transform.parent != null && body.HordeManager != null)
        {
            body.HordeManager.RemoveHordePuppet(body);
        }
        GameObject myNewHorde = new GameObject("PlayerHordeManager");
        body.HordeManager = myNewHorde.AddComponent<HordeManager>();
        body.HordeManager.InitAlpha(body);
        body.transform.parent = myNewHorde.transform;

        transform.parent = body.transform;
        transform.position = body.transform.position;
        gameObject.SetActive(false);
        GameManager.Instance.Possession(body);
        body.InitAction();
        if (body.Animator != null)
            body.Animator.SetTrigger("Revive");
        GameManager.Instance.PlayerBrain.host = this;

        GameManager.Instance.FeedbackManager.PossessBody(body.transform);
    }
    public void GoOutBody(Puppet body)
    {
        body.HordeManager.NeedNewAlpha();
        transform.SetParent(null);// = null;
        transform.position = body.transform.position;
        gameObject.SetActive(true);
        if (body.Life >0)
            body.PuppetAction = new DeathAction(body);

        GameManager.Instance.Possession(puppet);
        GameManager.Instance.FeedbackManager.UnPossessBody(transform);
        // si la horde est vide on la détruit.
        if (body.HordeManager.HordePuppets.Count == 0)
            GameManager.Instance.hordeCreator.DestroyHorde(body.HordeManager);
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
            if (deadBodyList.Count == 0 && OnDeadBodyClose!=null)
                OnDeadBodyClose(null);
                
            if(deadPuppet == closestDeadPuppet)
                closestDeadPuppet = null;
        }
    }
    public void Update()
    {
        if (deadBodyList.Count != 0)
        {
            ClosestDeadBody();
        }
        if (Input.GetButton("Fire1") && closestDeadPuppet != null)
        {
            GoInBody(closestDeadPuppet);
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

        if (closest != closestDeadPuppet)
        {
            closestDeadPuppet = closest;
            //Debug.Log("DeadBody = " + closest.gameObject.name);
            if (OnDeadBodyClose != null)
            {
                OnDeadBodyClose(closestDeadPuppet);
            }
        }
    }
}
