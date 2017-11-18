using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaGuide_State : Follow_State
{
    private Vector3 vectDirection;
    private float timerToApplyThisDirection;
    private float maxTimerToApplyThisDirection;
    private float maxAngleRandomToAppy;

    public AlphaGuide_State(Puppet _puppet) : base(_puppet)
    {
        
    }

    public override void OnBegin()
    {
        base.OnBegin();
        vectDirection = Vector3.zero;
        maxAngleRandomToAppy = 1.2f;
        FixedUpdateFct = LookingForDirection;
        timerForCheckingFoes = Random.value; // chaque puppet aura un timer différent => évite d'avoir tous les spherecast des puppet à la meme frame!
    }
    public override void OnEnd()
    {
        base.OnEnd();
    }

    private void GoToDirection()
    {
        timerToApplyThisDirection += Time.deltaTime;
        Move();
        if (timerToApplyThisDirection > maxTimerToApplyThisDirection)
        {
            FixedUpdateFct = LookingForDirection;
        }
    }
    private void LookingForDirection()
    {
        timerToApplyThisDirection = 0.0f;
        maxTimerToApplyThisDirection = Random.Range(5.0f, 20.0f);

        vectDirection = Vector3.Normalize(new Vector3(Random.Range(-100.0f, 100.0f), puppet.transform.position.y, Random.Range(-100.0f, 100.0f)));

        FixedUpdateFct = GoToDirection;
    }

    protected override void Move()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 newPoint = puppet.transform.position + (vectDirection * 3.0f) + (Vector3.Cross(vectDirection, Vector3.up) * Random.Range(-maxAngleRandomToAppy, maxAngleRandomToAppy));
        steering.Seek(newPoint, 1.0f);

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
