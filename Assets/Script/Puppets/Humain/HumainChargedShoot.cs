using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumainChargedShoot : HumainAction
{

    float chargeTime;
    float timer;
    bool canLauchAttack;
    Transform weaponOrigin;
    public HumainChargedShoot(Puppet _puppet) : base(_puppet)
    {
        weaponOrigin = puppet.transform.Find("WeaponOrigin");
    }
    public override  void OnBegin()
    {
        base.OnBegin();
        timer = 0;
        chargeTime = 0.75f;
        canLauchAttack = false;
    }
    public override void OnEnd()
    {
        base.OnEnd();

    }
    public void OnCharge()
    {
        timer += Time.deltaTime;
        if (timer > chargeTime)
        {
            canLauchAttack = true;
        }
    }


    public override void Charge()
    {
        // nop.
    }
    public override void Shoot()
    {
        if (canLauchAttack)
        {
            // shoot
        }
    }
    public override void RetreatAttack()
    {
        // why not. cracké
    }
}
