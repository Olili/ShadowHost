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

    public Puppet CurrentAlpha
    {
        get
        {
            return currentAlpha;
        }

        set
        {
            currentAlpha = value;
        }
    }
    #endregion
<<<<<<< refs/remotes/origin/master

    public void InitAlpha(Puppet _firstAlpha)
    {
        currentAlpha = _firstAlpha;
        AddHordePuppet(currentAlpha);
=======

    public void InitAlpha(Puppet _firstAlpha)
    {
        currentAlpha = _firstAlpha;
        AddHordePuppet(currentAlpha);
>>>>>>> Auto stash before rebase of "origin/master"
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
                pup.HordeManager = foeLeaderPuppet.HordeManager;
                pup.transform.parent = foeLeaderPuppet.HordeManager.transform;
            }
        }
        HordePuppets.Clear();

        //Destroy(this.gameObject);
    }
}