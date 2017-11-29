using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class ChaseSpider_State : Chase_State
//{
//    public ChaseSpider_State(Puppet _puppet) : base(_puppet)
//    {

//    }
//    public override void OnBegin()
//    {
//        base.OnBegin();
//        FixedUpdateFct = FixedUpdate_ChaseFoe;
//        UpdateFct = Update_ChaseFoe;
//        FindTheNearestFoe();
//    }
//    public override void OnEnd()
//    {
//        base.OnEnd();
//    }
//    public override void FixedUpdate_ChaseFoe()
//    {
//        base.FixedUpdate_ChaseFoe();

//        if (myTarget != null && !(myTarget.PuppetAction is DeathAction))
//        {
//            myVel = puppet.Rb.velocity;

//            Vector3 vecFromFoe = puppet.transform.position - myTarget.transform.position;

//            if (vecFromFoe.magnitude < 2.0f)
//            {
//                puppet.transform.LookAt(new Vector3(myTarget.transform.position.x, puppet.transform.position.y, myTarget.transform.position.z));
//                if (puppet.PuppetAction is SpiderAction)
//                    (puppet.PuppetAction as SpiderAction).BasicAttack();
//            }
//            else
//            {
//                steering.Seek(myTarget.transform.position, 0.7f);
//                Move();
//            }

//        }
//        else
//        {
//            FindTheNearestFoe();
//        }
//    }
//}
