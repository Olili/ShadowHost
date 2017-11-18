using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeManager : MonoBehaviour
{

    private Puppet foeLeaderPuppet;
    private List<Puppet> hordePuppets = new List<Puppet>();
    private Puppet currentAlpha;

    #region GetSet
    public List<Puppet> HordePuppets
    {
        get
        {
            return hordePuppets;
        }

        set
        {
            hordePuppets = value;
        }
    }
    public Puppet FoeLeaderPuppet
    {
        get
        {
            return foeLeaderPuppet;
        }

        set
        {
            foeLeaderPuppet = value;
        }
    }
    #endregion

    public void InitAlpha(Puppet _firstAlpha)
    {
        currentAlpha = _firstAlpha;
        AddHordePuppet(currentAlpha);
    }
    public void AddHordePuppet(Puppet puppet)
    {
        HordePuppets.Add(puppet);
    }
    public void RemoveHordePuppet(Puppet puppet)
    {
        HordePuppets.Remove(puppet);
    }

    public void NeedNewAlpha()
    {
        Puppet tempLeader = null;

        foreach (Puppet pup in HordePuppets)
        {
            if (pup != currentAlpha && !(pup.PuppetAction is DeathAction))
            {
                if (tempLeader == null)
                {
                    tempLeader = pup;
                    tempLeader.gameObject.AddComponent<Alpha>();
                    tempLeader.gameObject.GetComponent<Alpha>().Init();
                }
                pup.Leader = tempLeader;
            }
        }
        
    }
    public void TransmitHorde()
    {
        foreach (Puppet pup in HordePuppets)
        {
            if (pup != currentAlpha && !(pup.PuppetAction is DeathAction))
            {
                pup.Leader = foeLeaderPuppet;
                foeLeaderPuppet.HordeManager.AddHordePuppet(pup);
                pup.transform.parent = foeLeaderPuppet.HordeManager.transform;
            }
        }
        HordePuppets.Clear();

        //Destroy(this.gameObject);
    }


    private float timerBeforeDestroy = 0.0f;
    public void FixedUpdate()
    {
        if (hordePuppets.Count == 0)
        {
            timerBeforeDestroy += Time.deltaTime;
            if (timerBeforeDestroy > 1.5f)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            timerBeforeDestroy = 0.0f;
        }
    }
}
