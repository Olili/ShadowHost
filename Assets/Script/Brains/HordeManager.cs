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
    //public void SetNewLeader(Puppet _leader)
    //{
    //    currentAlpha = _leader;
    //    HordeManager newHorde = _leader.HordeManager;
    //    for (int i = 0; i < hordePuppets.Count; i++)
    //    {
    //        newHorde.AddHordePuppet(hordePuppets[i]);
    ////        hordePuppets[i].Leader = _leader;
    ////        hordePuppets[i].transform.parent = _leader.HordeManager.transform;
    ////        hordePuppets[i].HordeManager = _leader.HordeManager;
    ////    }
    //}
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
            if (pup != currentAlpha && !(pup.PuppetAction is DeathAction)) // Peut être ce cas ne doit pas arriver ? 
            {
                if (tempLeader == null)
                {
                    tempLeader = pup;
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
        // Comment ça se passe si c'est le dernier qui est tué.
        //Destroy(this.gameObject);
    }
}