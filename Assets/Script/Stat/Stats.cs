using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Stats
{
    public enum StatType
    {
        move_speed,
        maxAcceleration, // spécifique au steering ~= vitesse pour changer de direction.
        maxTurnSpeed, // il sera peut être interessant de la changer pour le joueur.
        strengh, // non implémenté

        max_stats
    }

    private List<StatBuff> buffList;
    [SerializeField] private Stat[] statTab = new Stat[(int)StatType.max_stats]; // tableau contenant toutes les stats du joueur

    public Stats()
    {
        for (int i = 0; i < (int)StatType.max_stats; i++)
        {
            Stat stat = new Stat(1, this, ((StatType)i).ToString());
            statTab[i] = stat;
        }
        buffList = new List<StatBuff>();
    }
    public void Init()
    {
        buffList = new List<StatBuff>();
        for (int i = 0; i < (int)StatType.max_stats; i++)
        {
            UpdateStat(statTab[i]);
            if (i == (int)StatType.maxAcceleration)
            {
                if (statTab[i].currentStat > 1)
                {
                    Debug.Log("Error : maxAcceleration shoudn't be > 1");
                }
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="statType"></param>
    /// <param name="isCurrent">Permet de récuperer la stat avec en prenant en compte les buff </param>
    /// <returns></returns>
    public float Get(StatType statType, bool isCurrent = true)
    {
        if (isCurrent)
            return statTab[(int)statType].currentStat;
        else
            return statTab[(int)statType].BaseStat;
    }


    /// <summary>
    /// Change the value of the BASE stat of player (without appying buff)
    /// </summary>
    /// <param name="statType"> </param>
    /// <param name="value"></param>
    public void Set(StatType statType, float value)
    {
        statTab[(int)statType].BaseStat = value;
    }
    public void AddBuff(StatBuff buff)
    {
        if (buffList == null)
            buffList = new List<StatBuff>();
        buffList.Add(buff);
        UpdateStat(buff.StatType);
        Debug.Log("Buff Added");
    }
    public void RemoveBuff(StatBuff buff)
    {
        StatType stat = buff.StatType;
        buffList.Remove(buff);
        UpdateStat(stat);
        Debug.Log("Buff Removed");
    }
    public  void UpdateStat(StatType stat)
    {
        float newStatValue = statTab[(int)stat].BaseStat;
        for (int i = 0; i < buffList.Count; i++)
        {
            if (buffList[i].StatType == stat)
            {
                newStatValue += buffList[i].Value;
            }
        }
        statTab[(int)stat].currentStat = newStatValue;
    }
    public void UpdateStat(Stat stat)
    {
        float newStatValue = stat.BaseStat;
        if (buffList!=null)
            for (int i = 0; i < buffList.Count; i++)
            {
                if (buffList[i].Stat == stat)
                {
                    newStatValue += buffList[i].Value;
                }
            }
        stat.currentStat = newStatValue;
    }
    public void Update()
    {
        if (buffList != null)
            for (int i = 0; i < buffList.Count; i++)
            {
                StatBuff buff = buffList[i];
                float timer = buff.UpdateTimer();
                if (timer != -1 && timer < 0)
                {
                    RemoveBuff(buff);
                }
            }
    }
}

