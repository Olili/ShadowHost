using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderFollow_State : Follow_State
{
    public SpiderFollow_State(Puppet _puppet) : base(_puppet)
    {
    }

    public override void OnBegin()
    {
        base.OnBegin();
        chillTest = false;
        FixedUpdateFct = FollowMovingPlayer;
    }
    public override void OnEnd()
    {
        base.OnEnd();
    }
}
