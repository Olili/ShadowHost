using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Stat
{
    [HideInInspector] public string name; // ne sert qu'a l'affichage dans l'éditor si une autre solution existe je suis chaud
    [SerializeField]private float baseStat;
    [HideInInspector] public float currentStat;
    [System.NonSerialized] private Stats statContainer;

    public float BaseStat
    {
        get
        {
            return baseStat;
        }

        set
        {
            baseStat = value;
            statContainer.UpdateStat(this);
            //Debug.Log("Yes i am here !!!");
        }
    }

    public Stat(float _baseStat, Stats _statContainer , string _statName)
    {
        statContainer = _statContainer;
        BaseStat = _baseStat;
        name = _statName;
    }
}
