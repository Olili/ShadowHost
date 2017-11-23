using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntFollow_State : Follow_State
{
    public GruntFollow_State(Puppet _puppet) : base(_puppet)
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
