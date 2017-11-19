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

    public ptrStateFct CurUpdateFct
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
    /// <summary>
    /// Permet de set la velocity du puppet
    /// Ne marche pas si puppet n'est pas sur le sol
    /// Si on veut être sur le controller la velocité passer
    /// par le RB direction.
    /// </summary>
    /// <param name="velocity"></param>
    public virtual void SetVelocity(Vector3 velocity)
    {
        if (velocity != Vector3.zero)
        {
            puppet.Rb.velocity = velocity;
        }
        else
        {
            if (puppet.IsOnGround)
                puppet.Rb.velocity = Vector3.zero; // peut être smooth mais en attendant c'est bim!
        }
    }
   
    public virtual void SetRotation(Vector3 direction)
    {
        if ((!puppet.IsOnGround))
            return;

        Vector3 right;
        if (direction.magnitude > 0.1f)
            right = Vector3.Cross(direction, puppet.OnPlanNormal);
        else  // si la vitesse de rotation est trop faible on garde son orientation.
            right = Vector3.Cross(puppet.transform.forward, puppet.OnPlanNormal);
        //  On oriente le puppet en fonction de la direction donnée et de l'orientation sur le plan
        Vector3 OnPlanDirection = Vector3.Cross(puppet.OnPlanNormal, right);
            // On tourne au fil du temps
        float speed = puppet.stats.Get(Stats.StatType.maxTurnSpeed) * Time.fixedDeltaTime;
        if (OnPlanDirection != Vector3.zero) 
        {
            Quaternion targetNormaRotation = Quaternion.LookRotation(OnPlanDirection, Vector3.up);
            Quaternion finalRotation = Quaternion.RotateTowards(puppet.transform.rotation, targetNormaRotation, speed);
            puppet.Rb.MoveRotation(finalRotation);
        }
    }
    public void RotateTowardGround()
    {
        float speed = puppet.stats.Get(Stats.StatType.maxTurnSpeed) * Time.fixedDeltaTime;
        Vector3 right = Vector3.Cross(puppet.transform.forward, Vector3.up);
        Vector3 planDirection = Vector3.Cross(Vector3.up, right);
        Quaternion targetNormaRotation = Quaternion.LookRotation(planDirection, Vector3.up);
        Quaternion finalRotation = Quaternion.RotateTowards(puppet.transform.rotation, targetNormaRotation, speed);
        puppet.Rb.MoveRotation(finalRotation);
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
            OnDeath();
        }
    }
    public virtual void OnDeath()
    {
        if (puppet.transform.parent.GetComponent<HordeManager>().HordePuppets.Contains(puppet))
            puppet.transform.parent.GetComponent<HordeManager>().RemoveHordePuppet(puppet);

        //puppet.gameObject.SetActive(false);
        if (puppet.GetComponent<Alpha>() != null)
        {
            if (puppet.GetComponent<IA_Brain>().MyIAState is Chase_State)
            {
                puppet.transform.parent.GetComponent<HordeManager>().NeedNewAlpha();
                GameObject.Destroy(puppet.gameObject.GetComponent<Alpha>());
            }
            else if(puppet.GetComponent<IA_Brain>().MyIAState is AlphasFight_State && puppet.HordeManager.FoeLeaderPuppet.GetComponent<Alpha>() != null)
            {
                puppet.transform.parent.GetComponent<HordeManager>().TransmitHorde();
                GameObject.Destroy(puppet.gameObject.GetComponent<Alpha>());
            }
        }   
        if (puppet.gameObject.GetComponent<Brain>() != null && !(puppet.gameObject.GetComponent<Brain>() is PlayerBrain))
        {
            GameObject.Destroy(puppet.gameObject.GetComponent<Brain>());
        }
        if(puppet.transform.parent.GetComponent<HordeManager>().HordePuppets.Count==0)
        {
            GameObject tempManagerToRemove = puppet.transform.parent.GetComponent<HordeManager>().gameObject;
            puppet.transform.parent = null;
            GameObject.Destroy(tempManagerToRemove);
        }
        else
        {
            puppet.transform.parent = null;
        }

        puppet.PuppetAction = new DeathAction(puppet);
    }
    public virtual void BasicAttack()
    {

    }
    public virtual void DrawGizmo()
    {

    }
    public virtual void OnAnimationEvent()
    {

    }
    public virtual void TriggerEnter(Collider other)
    {}
    public virtual void TriggerStay(Collider other)
    {}
    public virtual void TriggerExit(Collider other)
    {}
}
