using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Steering))]
public class Puppet : MonoBehaviour {

    public Stats stats = new Stats();
    PhysicMaterial physicMaterial;
    private Rigidbody rb;
    Vector3 extents;
    public Puppet leader;

    public Vector3 Extents
    {
        get{return extents;}
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        physicMaterial = GetComponent<Collider>().material;
        stats.Init();
        extents = GetComponent<Collider>().bounds.extents;
    }

    public virtual void Move(Vector3 direction, float factor)
    {
        Vector3 translation = direction * 1 * stats.Get(Stats.StatType.move_speed) * Time.fixedDeltaTime;
        rb.MovePosition(rb.transform.position + translation);
    }
    public virtual void SetVelocity(Vector3 velocity)
    {
        Vector3 rbVelocity = rb.velocity;
        if (velocity != Vector3.zero)
        {
            physicMaterial.dynamicFriction = 0;
            rb.velocity = velocity ;
        }
        else
        {
            physicMaterial.dynamicFriction = 0.5f;
        }
    }
    public void Rotate(Vector3 direction)
    {
        Vector3 groundDirection = new Vector3(direction.x, 0, direction.z);
        float speed = stats.Get(Stats.StatType.maxTurnSpeed) * Time.fixedDeltaTime;
        if (groundDirection.magnitude > 0.1f)
        {
            Quaternion targetNormaRotation = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion finalRotation = Quaternion.RotateTowards(transform.rotation, targetNormaRotation, speed);
            rb.MoveRotation(finalRotation);
        }
    }
    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }
    public virtual void OnTakeDamage()
    {

    }

}
