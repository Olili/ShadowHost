using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeManager : MonoBehaviour
{

    [SerializeField] private Puppet foeLeaderPuppet;
    [SerializeField] private List<Puppet> hordePuppets = new List<Puppet>();
    [SerializeField] private Puppet currentAlpha;

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
            if (pup != currentAlpha) 
            {
                if (tempLeader == null)
                {
                    tempLeader = pup;
                }
                pup.Leader = tempLeader;
            }
        }
            // si on a trouvé un leader et que ce n'est pas le joueur
        if (tempLeader != null && tempLeader.GetComponent<PlayerBrain>() == null)
        {
            IA_Brain ia_Brain = tempLeader.GetComponent<IA_Brain>();
            ia_Brain.MyIAState = ia_Brain.GetTypeState(Brain.E_State.follow);
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