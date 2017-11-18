using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumainChargedShoot : HumainAction
{

    float chargeTime;
    float timer;
    bool canLauchAttack;
    public HumainChargedShoot(Puppet _puppet) : base(_puppet)
    {
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


    public virtual void Charge()
    {
        // nop.
    }
    public virtual void Shoot()
    {
        if (canLauchAttack)
        {
            // shoot
        }
    }
    public virtual void RetreatAttack()
    {
        // why not. cracké
    }
}
