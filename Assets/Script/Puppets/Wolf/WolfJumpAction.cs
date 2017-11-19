using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfJumpAction : WolfAction
{
    float timer;
    float chargeTime;
    bool attackStarted;
    Vector3 attackExtents;
    Vector3 attackOrigin;
    float pushForce;
    public WolfJumpAction(Puppet _puppet) : base(_puppet)
    {

    }
    public override void OnBegin()
    {
        base.OnBegin();
        timer = 0;
        chargeTime = 0.1f;
        pushForce = 5;
        attackStarted = false;
        CurUpdateFct = Charge;
        puppet.Animator.SetTrigger("StartAttack");
        attackExtents = puppet.Extents;
        attackOrigin = puppet.centerDown.position;

    }
    public override void OnEnd()
    {
        base.OnEnd();
    }

    public void Charge()
    {
        puppet.Rb.velocity = Vector3.zero;
        timer += Time.deltaTime;


        if (timer > chargeTime)
        {
            timer = 0;
            CurUpdateFct = DuringAttack;
        }
    }
    public void DuringAttack()
    {

        puppet.Rb.velocity = puppet.stats.Get(Stats.StatType.move_speed) * puppet.transform.forward * 1.5f;
        timer += Time.deltaTime;


        if (puppet.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
        {
            attackStarted = true;
        }
        if (attackStarted && puppet.Animator.IsInTransition(0))
        {
            timer = 0;
            attackStarted = false;
            CurUpdateFct = AttackEnd;
            puppet.Animator.SetFloat("Velocity", 0);
        }
    }
    public void AttackEnd()
    {
        puppet.Rb.velocity = Vector3.zero;
        timer += Time.deltaTime;
        if (timer > 0.2f)
        {
            CurUpdateFct = null;
            puppet.PuppetAction = new GruntAction(puppet); // On laisse la main
        }
    }
 
    // autres

    public override void OnAnimationEvent()
    {
        puppet.GetComponentInChildren<ParticleSystem>().Play();
        puppet.AttackCollision(attackExtents,attackOrigin, pushForce);
    }

    // Les ordres/transition des brain   :
    public override void JumpAttack()
    {
        // not attack
    }

    public override void SetVelocity(Vector3 velocity)
    {
        // interdiction de bouger
    }
    public override void SetRotation(Vector3 direction)
    {
        // interdiction de tourner
    }
   
  
}
