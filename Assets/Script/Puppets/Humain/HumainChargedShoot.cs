using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumainChargedShoot : HumainAction
{

    float chargeTime;
    float timer;
    bool canLauchAttack;
    Transform weaponOrigin;
    bool animationStarted;
    public HumainChargedShoot(Puppet _puppet) : base(_puppet)
    {
        
    }
    public override  void OnBegin()
    {
        base.OnBegin();
        Debug.Log("OnBegin");
        timer = 0;
        chargeTime = 0.2f;
        canLauchAttack = false;
        weaponOrigin = puppet.transform.Find("WeaponOrigin");
        puppet.Animator.SetTrigger("Charge");
        CurUpdateFct = OnCharge;
        animationStarted = false;
    }
    public override void OnEnd()
    {
        base.OnEnd();

    }
    public void OnCharge()
    {
        timer += Time.deltaTime;
        if (timer > chargeTime && canLauchAttack)
        {
            //Debug.Log("OnCharge");
            puppet.Animator.SetTrigger("Release");
            CurUpdateFct = OnRelease;
        }
    }
    public void OnRelease()
    {
        if (puppet.Animator.GetCurrentAnimatorStateInfo(0).IsName("AttackChargeEnd"))
        {
            animationStarted = true;
        }
        if (animationStarted && puppet.Animator.IsInTransition(0))
        {
            animationStarted = false;
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
            puppet.PuppetAction = new HumainAction(puppet); // On laisse la main
        }
    }


        /// overrride

    public override void Charge()
    {
        canLauchAttack = false;
    }
    public override void Shoot()
    {
        canLauchAttack = true;
    }
    public override void RetreatAttack()
    {
        // why not. cracké
    }
}
