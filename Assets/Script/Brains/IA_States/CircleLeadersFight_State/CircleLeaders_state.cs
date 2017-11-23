using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLeaders_state : IA_State
{
    protected float rotationDirection; // positive or negative
    protected float distanceFromFight;
    protected float speed; // init differement pour chaque espece (donc chez les enfants)

    public CircleLeaders_state(Puppet _puppet) : base(_puppet)
    {
    }
    public override void OnBegin()
    {
        base.OnBegin();
        rotationDirection = (Random.value <= 0.5f) ? -1.0f : 1.0f;
        distanceFromFight = Random.Range(10.0f, 15.0f);
        FixedUpdateFct = Rotate;
    }
    public override void OnEnd()
    {
        base.OnEnd();
    }

    protected void Rotate()
    {
        Vector3 myVel = puppet.Rb.velocity;

        Vector3 tempDirection = Vector3.Normalize(Vector3.Cross(puppet.transform.position - puppet.Leader.transform.position, Vector3.up));

        Vector3 direction = (tempDirection * 3.0f);
        Vector3 point = puppet.transform.position + direction;

        float deltaDistanceFromFight = Vector3.Distance(point, puppet.Leader.transform.position) - distanceFromFight;
        Vector3 finalPoint = point + (deltaDistanceFromFight * Vector3.Normalize(puppet.Leader.transform.position - point));
        steering.Seek(finalPoint, 0.7f);
        Move();

        if (puppet.Leader.GetComponent<IA_Brain>() == null)
        {
            Debug.Log("poijnt d'arret");
        }

        if (!(puppet.Leader.GetComponent<IA_Brain>().MyIAState is AlphasFight_State))
        {
            puppet.GetComponent<IA_Brain>().MyIAState = puppet.GetComponent<IA_Brain>().GetTypeState(Brain.E_State.follow);
        }
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
