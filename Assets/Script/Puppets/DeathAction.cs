﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAction : PuppetAction
{
    Animator animator;
    public DeathAction(Puppet _puppet) : base(_puppet)
    {
        animator = _puppet.Animator;
        CurFixedUpdateFct = OnDeadBody;
    }
    public override void OnBegin()
    {
        base.OnBegin();
        // lancement animation mort. 
        if (animator!=null)
        {
            animator.SetTrigger("Death");
        }
        // Creation / activation trigger stuff.
    }
    public void OnDeadBody()
    {
        puppet.Rb.velocity = Vector3.zero;
        Host host = GameManager.Instance.PlayerBrain.GetComponent<Host>();
        if (host!=null)
        {
            float distance = Vector3.Distance(host.transform.position , puppet.transform.position);
            if (distance < 3)
            {
                host.AddBody(puppet);
            }
            else
            {
                host.RemoveBody(puppet);
            }
        }
    }
    
    public override void OnEnd()
    {
        base.OnEnd();
    }
    public override void OnHit(float damage, Vector3 force)
    {
    }
    public override void SetRotation(Vector3 direction)
    {
        base.SetRotation(direction);
    }
    public override void SetVelocity(Vector3 velocity)
    {
        base.SetVelocity(velocity);
    }
}