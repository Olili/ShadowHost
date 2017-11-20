using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAction : PuppetAction
{
    Animator animator;
    float timer;
    public DeathAction(Puppet _puppet) : base(_puppet)
    {
        animator = _puppet.Animator;
        CurFixedUpdateFct = OnDeadBody;
        puppet.GetComponent<Collider>().enabled = false;
        timer = 0;
    }
    public override void OnBegin()
    {
        base.OnBegin();
        // lancement animation mort. 
        if (animator!=null)
        {
            animator.SetTrigger("Death");
        }
       
        puppet.Leader = null;
        GameManager.Instance.hordeCreator.AddDeadPuppet(puppet);
    }
    public void OnDeadBody()
    {
        timer += Time.deltaTime;
        puppet.Rb.velocity = Vector3.zero;
        puppet.Rb.angularVelocity = Vector3.zero;
        Host host;
        if (GameManager.Instance.PlayerBrain != null)
        {
            host = GameManager.Instance.PlayerBrain.GetComponent<Host>();
            if (host != null)
            {
                float distance = Vector3.Distance(host.transform.position, puppet.transform.position);
                if (distance < 3)
                {
                    if (timer>1)
                        host.AddBody(puppet);
                }
                else
                {
                    host.RemoveBody(puppet);
                }
            }
        }
    }
    public override void OnDeath()
    {
        Debug.Log("Error, trying to kill dead stuff");
    }
    public override void OnEnd()
    {
        base.OnEnd();
        puppet.GetComponent<Collider>().enabled = true;
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
