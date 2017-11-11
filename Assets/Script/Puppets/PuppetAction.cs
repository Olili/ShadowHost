using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 /*
  * C'est la classe mère de toutes les puppetState
  * Une puppetState implemente les action spécifique des puppets
  * Elle permet de demander à la creature de faire une action.
  * ex : SetVelocity / Attaque ... 
  * 
  * Dans une puppetState on trouve : l'action faite par la creature.
  * On trouve aussi les transitions autorisée dans l'action
  * ex : Si on est en train d'attaquer on va ignorer un ordre de mouvement.
 */


public delegate void ptrStateFct();
public class PuppetAction  {

    private ptrStateFct curUpdateFct;
    private ptrStateFct curFixedUpdateFct;
    protected Puppet puppet;

#region getterSetters

    public ptrStateFct CurActionFct
    {
        get
        {
            return curUpdateFct;
        }

        protected set
        {
            curUpdateFct = value;
        }
    }

    public ptrStateFct CurFixedUpdateFct
    {
        get
        {
            return curFixedUpdateFct;
        }

        set
        {
            curFixedUpdateFct = value;
        }
    }

    #endregion
    public PuppetAction(Puppet _puppet)
    {
        puppet = _puppet;
    }

    public virtual void OnBegin()
    {

    }
    public virtual void OnEnd()
    {

    }
    public virtual void SetVelocity(Vector3 velocity)
    {
        Vector3 rbVelocity = puppet.Rb.velocity;
        if (velocity != Vector3.zero)
        {
            puppet.PhysicMaterial.dynamicFriction = 0;
            puppet.Rb.velocity = velocity;
        }
        else
        {
            puppet.PhysicMaterial.dynamicFriction = 0.5f;
        }
    }
    public virtual void SetRotation(Vector3 direction)
    {
        Vector3 groundDirection = new Vector3(direction.x, 0, direction.z);
        float speed = puppet.stats.Get(Stats.StatType.maxTurnSpeed) * Time.fixedDeltaTime;
        if (groundDirection.magnitude > 0.1f)
        {
            Quaternion targetNormaRotation = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion finalRotation = Quaternion.RotateTowards(puppet.transform.rotation, targetNormaRotation, speed);
            puppet.Rb.MoveRotation(finalRotation);
        }
    }
    public virtual void OnUpdate()
    {
        if (curUpdateFct != null)
            curUpdateFct();
    }
    public virtual void OnFixedUpdate()
    {
        if (CurFixedUpdateFct != null)
            CurFixedUpdateFct();
    }
    public virtual void OnHit(float damage, Vector3 force)
    {
        puppet.Life -= damage;
        puppet.Rb.AddForce(force, ForceMode.Impulse);
        if (puppet.Life <= 0)
        {
            if (puppet.Leader !=null)
            {
                puppet.Leader.GetComponent<Alpha>().RemoveHordePuppet(puppet);
            }

            GameObject.Destroy(puppet.gameObject);
        }
    }
    public virtual void DrawGizmo()
    {

    }
    public virtual void OnAnimationEvent()
    {

    }
}
