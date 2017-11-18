using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase_State : IA_State
{
    protected Puppet foeLeaderPuppet;
    protected Puppet myTarget = null;
    protected Vector3 myVel = Vector3.zero;

    public Chase_State(Puppet _puppet) : base(_puppet)
    {
        foeLeaderPuppet = puppet.HordeManager.FoeLeaderPuppet;
    }

    public override void OnBegin()
    {
        base.OnBegin();
    }
    public override void OnEnd()
    {
        base.OnEnd();
    }

    public virtual void FixedUpdate_ChaseFoe()
    {
        if (myTarget == null && !FindTheNearestFoe())
        {
            Debug.Log("Pas d'ennemie trouvé => change State to follow");
        }
    }
    public virtual void Update_ChaseFoe()
    {

    }
    protected virtual void Move()
    {
    }

    protected virtual bool FindTheNearestFoe()
    {
        return false;
    }
}
