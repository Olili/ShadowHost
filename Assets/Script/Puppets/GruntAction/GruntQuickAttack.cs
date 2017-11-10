using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntQuickAttack : GruntAction
{
    float timer = 0;
    float chargeTime = 0;
    Vector3 attackExtents;


    public GruntQuickAttack(Puppet _puppet) : base(_puppet)
    {
        attackExtents = new Vector3(1, 0.5f, 4);
    }
            // La state machine de QuickAttack   :
    public override void OnBegin()
    {
        base.OnBegin();
        CurActionFct = Charge;
    }
    public override void OnEnd()
    {
        base.OnEnd();
    }

    public void Charge()
    {
        puppet.Rb.velocity = Vector3.zero;
        timer += Time.deltaTime;
        if (timer> chargeTime)
        {
            timer = 0;
            CurActionFct = DuringAttack;
            puppet.Animator.SetTrigger("StartAttack");
            puppet.GetComponentInChildren<ParticleSystem>().Play();
            AttackCollision();
        }
    }
    public void DuringAttack()
    {

        puppet.Rb.velocity = puppet.stats.Get(Stats.StatType.move_speed)* puppet.transform.forward*2;
        timer += Time.deltaTime;
        if (timer > 0.5f)
        {
            timer = 0;
            CurActionFct = AttackEnd;

         
        }
    }
    public void AttackEnd()
    {
        puppet.Rb.velocity = Vector3.zero;
        timer += Time.deltaTime;
        if (timer > 0.1f)
        {
            CurActionFct = null;
            puppet.PuppetAction = new GruntAction(puppet); // On laisse la main
        }
    }

        // autres
    public void AttackCollision()
    {
        int mask = LayerMask.GetMask(new string[] { "Puppet" });
        Vector3 attackCenter = puppet.transform.position + puppet.transform.forward* attackExtents.z;
        Collider[] collTab = Physics.OverlapBox(attackCenter, attackExtents, puppet.transform.rotation, mask);
        cubeCenter = attackCenter;
        for (int i = 0; i < collTab.Length;i++)
        {
            if (collTab[i].transform != puppet.transform)
            {
                Vector3 forceApply = collTab[i].transform.position - puppet.transform.position;
                Puppet targetPuppet = collTab[i].GetComponent<Puppet>();
                targetPuppet.PuppetAction.OnHit(puppet.stats.Get(Stats.StatType.strengh), forceApply.normalized * 5);
            }
        }
    }

        // Les ordres/transition des brain   :

    public override void OnChargeAttack()
    {
        // interdiction de charger Une attaque
    }
    public override void SetVelocity(Vector3 velocity)
    {
        // interdiction de bouger
    }
    public override void SetRotation(Vector3 direction)
    {
        // interdiction de tourner
    }
    public override void OnSimpleAttack()
    {
        // Interdiction d'attaquer
    }
    // Gzimo debug
    Vector3 cubeCenter;
    public override void DrawGizmo()
    {
        base.DrawGizmo();
        GL.PushMatrix();
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(cubeCenter, puppet.transform.rotation, puppet.transform.lossyScale);
        Gizmos.matrix = rotationMatrix;
        Gizmos.color = new Color(0, 0, 1, 0.5f);
       // Gizmos.DrawCube(cubeCenter, attackExtents);
        Gizmos.DrawCube(Vector3.zero, attackExtents);
        //Gizmos.DrawCube()
        GL.PopMatrix();
    }
}
