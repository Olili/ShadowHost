using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStunAction : PuppetAction
{
    float timer;
    Vector3 pushVelocity;
    public OnStunAction(Puppet _puppet,Vector3 _pushVelocity) : base(_puppet)
    {
        pushVelocity = _pushVelocity;
    }
    public override void OnBegin()
    {
        base.OnBegin();
        timer = pushVelocity.magnitude;
        CurUpdateFct = Wait;
        CurFixedUpdateFct = Pushed;
    }
    public override void OnEnd()
    {
        base.OnEnd();
    }
    public void Pushed()
    {
        puppet.Rb.velocity = pushVelocity;
    }
    public void Wait()
    {
        timer -= Time.deltaTime*30;
        if (timer < 0)
        {
            puppet.InitAction();
        }
    }
    public override void SetRotation(Vector3 direction)
    {
    }
    public override void SetVelocity(Vector3 velocity)
    {
    }
}
