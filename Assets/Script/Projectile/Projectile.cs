using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int damage;
    public bool isLauched;
    public Puppet laucher;
    public int speed;
    Vector3 direction;
    float pushForce;

    public void Init()
    {
        isLauched = false;
        Physics.IgnoreCollision(GetComponent<Collider>(), laucher.GetComponent<Collider>());
    }
    public void Update()
    {
        if (isLauched)
            transform.position += direction * speed;
    }
    public void Lauch(Vector3 _direction)
    {
        isLauched = true;
        this.direction = _direction;
    }
    public void OnTriggerEnter(Collider collider)
    {
        if (isLauched)
        {
            Puppet target = collider.GetComponent<Puppet>();
            if (target!=null)
            {
                laucher.HitPuppet(collider, pushForce);
            }
        }
    }
}
