using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphasFight_State : IA_State
{
    protected Puppet alphaOpposent;

    public AlphasFight_State(Puppet _puppet) : base(_puppet)
    {
    }
    public override void OnBegin()
    {
        base.OnBegin();
        alphaOpposent = puppet.HordeManager.FoeLeaderPuppet;
        FixedUpdateFct = GoToMyOpponent;
    }
    public override void OnEnd()
    {
        base.OnEnd();
    }

    public virtual void GoToMyOpponent()
    {

    }
    protected virtual void Move()
    {

    }
    public virtual void Fight()
    {

    }
}
