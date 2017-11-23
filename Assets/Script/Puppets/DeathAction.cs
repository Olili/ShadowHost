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
    }
    public override void OnBegin()
    {
        base.OnBegin();
        // lancement animation mort. 
        if (animator!=null)
        {
            animator.SetTrigger("Death");
        }
        //puppet.GetComponent<Collider>().enabled = false;
        //puppet.GetComponent<Collider>().se
        puppet.gameObject.layer = LayerMask.NameToLayer("Dead_Puppet");
        timer = 0;
        puppet.Leader = null;
        puppet.IsOnGround = false;
        GameManager.Instance.hordeCreator.AddDeadPuppet(puppet);
        CurFixedUpdateFct = OnDeadBody;
    }
    public void OnDeadBody()
    {
        timer += Time.deltaTime;
        if (timer >0.5f)
        {
            puppet.Rb.velocity = new Vector3(0,Mathf.Max(puppet.Rb.velocity.y,-8),0);
            puppet.Rb.angularVelocity = Vector3.zero;
        }
      
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
    public override void OnDeath(Puppet hitter)
    {
        Debug.Log("Error, trying to kill dead stuff");
    }
    public override void OnEnd()
    {
        base.OnEnd();
        puppet.gameObject.layer = LayerMask.NameToLayer("Puppet");
        //puppet.GetComponent<Collider>().enabled = true;
    }
    public override void OnHit(float damage, Vector3 force, Puppet hitter = null)
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
