using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeManager : MonoBehaviour
{
    private Puppet foeLeaderPuppet;

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
}
