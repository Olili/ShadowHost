using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int damage;
    public bool isLauched;
    public GameObject laucher;

    public void Init()
    {
        isLauched = false;
        Physics.IgnoreCollision(GetComponent<Collider>(), laucher.GetComponent<Collider>());
    }
    public void Lauch(Vector3 direction)
    {
        isLauched = true;
    }
    public void OnTriggerEnter(Collision collision)
    {

    }
}
