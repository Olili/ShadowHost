using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeManager : MonoBehaviour
{

    [SerializeField] private Puppet foeLeaderPuppet;
    [SerializeField] private List<Puppet> hordePuppets = new List<Puppet>();
    [SerializeField] private Puppet currentAlpha;



    bool isAlphaFighting;

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
        if (_firstAlpha != currentAlpha)
        {
            GameManager.Instance.FeedbackManager.NewAlpha(_firstAlpha.transform);
        }
        currentAlpha = _firstAlpha;
        AddHordePuppet(currentAlpha);
    }
    public void AddHordePuppet(Puppet puppet)
    {
        if (currentAlpha == null)
            Debug.LogError("l'alpha n'est pas set");
        puppet.transform.parent = transform;
        puppet.Leader = currentAlpha;
        if (!hordePuppets.Contains(puppet))
        {
            HordePuppets.Add(puppet);
        }
        puppet.HordeManager = this;
    }
    public void RemoveHordePuppet(Puppet puppet)
    {
        if (puppet == currentAlpha)
        {
            GameManager.Instance.FeedbackManager.NoAlpha(puppet.transform);
        }
        HordePuppets.Remove(puppet);
    }

    public void NeedNewAlpha()
    {
        Puppet tempLeader = null;


        foreach (Puppet pup in HordePuppets)
        {
            if (tempLeader == null)
            {
                InitAlpha(pup);
                tempLeader = pup;
            }
            AddHordePuppet(pup);
        }
            // si on a trouvé un leader et que ce n'est pas le joueur
        if (tempLeader != null && tempLeader.GetComponent<PlayerBrain>() == null)
        {
            IA_Brain ia_Brain = tempLeader.GetComponent<IA_Brain>();
            ia_Brain.MyIAState = ia_Brain.GetTypeState(Brain.E_State.follow);
        }

    }
    public void TransmitHorde(Puppet newLeader)
    {
        foreach (Puppet pup in HordePuppets)
        {
            if (pup != currentAlpha && !(pup.PuppetAction is DeathAction))
            {
                newLeader.HordeManager.AddHordePuppet(pup);
                pup.GetComponent<IA_Brain>().MyIAState = pup.GetComponent<IA_Brain>().GetTypeState(Brain.E_State.follow);
            }
        }
        HordePuppets.Clear();
        // Comment ça se passe si c'est le dernier qui est tué.
        //Destroy(this.gameObject);
    }
    /// <summary>
    /// Fonction qui permet à une puppet de prevenir l'aplha qu'elle a vu un ennemie.
    /// </summary>
    /// <param name="_FoePuppet"> puppet ennemie detecté </param>
    public void PuppetFindFoe(Puppet _foePuppet)
    {
        FoeLeaderPuppet = _foePuppet;
        foreach (Puppet myFollowers in HordePuppets)
        {
            Brain brain = myFollowers.GetComponent<Brain>();
            if (brain is IA_Brain)
                (brain as IA_Brain).MyIAState = (brain as IA_Brain).GetTypeState(Brain.E_State.chase);
        }
    }
    public void AlphaFight(Puppet _foePuppet)
    {
        FoeLeaderPuppet = _foePuppet;
        foreach (Puppet myFollowers in HordePuppets)
        {
            Brain brain = myFollowers.GetComponent<Brain>();
            if (brain is IA_Brain)
                (brain as IA_Brain).MyIAState = (brain as IA_Brain).GetTypeState(Brain.E_State.alphaFight);
        }
    }
}