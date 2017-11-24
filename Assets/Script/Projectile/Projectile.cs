using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public bool isLauched;
    public Puppet launcher;
    public float speed ;
    Vector3 direction;
    float pushForce;
    float distance;

    public void Init(Puppet _laucher)
    {
        isLauched = false;
        pushForce = 0;
        distance = 0;
        launcher = _laucher;
        Physics.IgnoreCollision(GetComponent<Collider>(), launcher.GetComponent<Collider>());
    }
    public void Update()
    {
        if (isLauched)
        {
            transform.position += direction * speed * Time.deltaTime;
            distance += speed * Time.deltaTime;
        }
        if (distance >40)
        {
            GameManager.Instance.ProjectilePool.SendToPool(this.gameObject);
        }
    }
    public void Lauch(Vector3 _direction,float _speed)
    {
        isLauched = true;
        this.direction = _direction;
        speed = _speed;
        transform.rotation = Quaternion.LookRotation(direction, launcher.OnPlanNormal);
    }
    public void OnTriggerEnter(Collider collider)
    {
        if (isLauched)
        {
            Puppet target = collider.GetComponent<Puppet>();
            if (target!=null)
            {
                if (target.Type != launcher.Type)
                {
                    launcher.HitPuppet(collider, pushForce, launcher);
                    GameManager.Instance.ProjectilePool.SendToPool(this.gameObject);
                }
                else if (target.FriendlyFire && launcher.FriendlyFire)
                {
                    launcher.HitPuppet(collider, pushForce, launcher);
                    GameManager.Instance.ProjectilePool.SendToPool(this.gameObject);
                }
            }
        }
    }
}
