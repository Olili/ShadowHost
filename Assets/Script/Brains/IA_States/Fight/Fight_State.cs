using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_State : IA_State
{
    public Puppet myTarget = null;
    float attackDistance;

    public Fight_State(Puppet _puppet) : base(_puppet)
    {
    }

    public override void OnBegin()
    {
        attackDistance = 3;
        if (myTarget.Type == puppet.Type)
            puppet.FriendlyFire = true;
        FixedUpdateFct = MoveToTarget;
        base.OnBegin();
    }
    public override void OnEnd()
    {
        puppet.FriendlyFire = false;
        base.OnEnd();
    }

    public void Transition()
    {
        if (myTarget == null || myTarget.PuppetAction is DeathAction)
        {
                // si le leader est loin : Follow Leader ( a implementer une fois que tout le monde marche.
            //if (Vector3.Distance(puppet.Leader.transform.position,puppet.transform.position)>20)
            //{
            //    //IA_Brain ia_Brain = puppet.brain as IA_Brain;
            //    //ia_Brain.MyIAState = ia_Brain.GetTypeState(Brain.E_State.follow);
            //}
            //else // sinon Je cherche une nouvelle cible. 
            (puppet.brain as IA_Brain).MyIAState = (puppet.brain as IA_Brain).GetTypeState(Brain.E_State.chase);
        }
    }
    public virtual void MoveToTarget()
    {
        if (Vector3.Distance(myTarget.transform.position, puppet.transform.position) > attackDistance)
            steering.Seek(myTarget.transform.position);
        else
            FixedUpdateFct = Attack;
        Move();
        Transition();
    }
    public virtual void Rotate()
    {
        Vector3 velocity = puppet.GetVelocity();
        velocity.y = 0;

    }
    public virtual void Attack()
    {
        if (Vector3.Distance(myTarget.transform.position, puppet.transform.position) < attackDistance)
            puppet.PuppetAction.BasicAttack();
        else
            FixedUpdateFct = MoveToTarget;
        Transition();
    }
    protected void Move()
    {
        Vector3 velocity = Vector3.zero;
        steering.Separation(0.7f);
        velocity = steering.ComputedVelocity;

        if (velocity.magnitude > 0.3f)
        {
            puppet.PuppetAction.SetVelocity(velocity);
            velocity.y = 0;
            puppet.PuppetAction.SetRotation(velocity.normalized);
        }
        else
        {
            puppet.PuppetAction.SetVelocity(Vector3.zero);
            puppet.PuppetAction.SetRotation(Vector3.zero);
        }
    }

}
