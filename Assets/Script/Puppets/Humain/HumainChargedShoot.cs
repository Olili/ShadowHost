using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class HumainChargedShoot : HumainAction
{

    float chargeTime;
    float timer;
    bool canLauchAttack;
    Transform weaponOrigin;
    bool animationStarted;
    Transform lanceOrigin;

    public HumainChargedShoot(Puppet _puppet) : base(_puppet)
    {
        lanceOrigin = puppet.FindChildByName("LanceOrigin", puppet.transform);
    }
    public override  void OnBegin()
    {
        base.OnBegin();
        timer = 0;
        chargeTime = 0.2f;
        canLauchAttack = false;
        weaponOrigin = puppet.transform.Find("WeaponOrigin");
        puppet.Animator.SetTrigger("Charge");
        CurUpdateFct = OnCharge;
        animationStarted = false;
        //Debug.Log("Begin State");
    }
    public override void OnEnd()
    {
        base.OnEnd();

    }
    public void OnCharge()
    {
        puppet.Rb.velocity = Vector3.zero;
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

    public override void OnAnimationEvent(string functionName)
    {
        //Debug.Log(functionName);
        //Get the method information using the method info class
        MethodInfo mi = this.GetType().GetMethod(functionName);
        //Invoke the method
        // (null- no parameter for the method call
        // or you can pass the array of parameters...)
        mi.Invoke(this, null);
    }
    public void OnLauchEvent()
    {
        //Debug.Log("je suis heureux");
        Projectile projectile  = GameManager.Instance.ProjectilePool.GetProjectile(ProjectileType.lance,puppet, lanceOrigin.position, lanceOrigin.rotation);
        projectile.Lauch(puppet.transform.forward, 20);
    }

    /// overrride
    public override void SetVelocity(Vector3 velocity)
    {

    }
    public override void Charge()
    {
        canLauchAttack = false;
        //Debug.Log("PushBack State");

    }
    public override void Shoot()
    {
        canLauchAttack = true;
        //Debug.Log("Unleash");

    }
    public override void RetreatAttack()
    {
        // why not. cracké
    }
}
