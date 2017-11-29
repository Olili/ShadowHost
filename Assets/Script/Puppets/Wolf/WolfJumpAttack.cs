using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfJumpAttack : WolfAction
{
    bool animationStarted;
    Vector3 attackExtents;
    Vector3 attackOrigin;
    float pushForce;

    public WolfJumpAttack(Puppet _puppet) : base(_puppet)
    {
    }
    public override void OnBegin()
    {
        base.OnBegin();
        animationStarted = false;
        CurFixedUpdateFct = OnCharge;
        puppet.Animator.SetTrigger("Attack");
    }
    public override void OnEnd()
    {
        base.OnEnd();
    }

    public void OnCharge()
    {
        puppet.Rb.velocity = Vector3.zero;
        if (puppet.Animator.GetCurrentAnimatorStateInfo(0).IsName("BeforeJump"))
        {
            animationStarted = true;
        }
        if (animationStarted && !puppet.Animator.GetCurrentAnimatorStateInfo(0).IsName("BeforeJump"))
        {
            animationStarted = false;
            CurFixedUpdateFct = OnJumpAttack;
        }
    }
    public void OnJumpAttack()
    {
        puppet.Rb.velocity = puppet.stats.Get(Stats.StatType.move_speed) * puppet.transform.forward * 1.5f;
        if (puppet.Animator.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack"))
        {
            animationStarted = true;
        }
        if (animationStarted && puppet.Animator.IsInTransition(0))
        {
            animationStarted = false;
            puppet.ResetAttackedCollidedPuppets();
            puppet.PuppetAction = new WolfAction(puppet);
        }
        puppet.AttackCollision(puppet, new Vector3(1, 1, 1), puppet.transform.position, 12);
    }

        // override actions : 

    public override void JumpAttack()
    {
        // interdit on fait rien.
    }
    //public override void SetRotation(Vector3 direction)
    //{
    //    // interdit
    //}
    public override void SetVelocity(Vector3 velocity)
    {
        // interdit
    }
}
