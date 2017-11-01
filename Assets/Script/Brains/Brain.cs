using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    protected Puppet puppet;

    // Use this for initialization
    public virtual void Awake()
    {
        puppet = GetComponent<Puppet>();
    }
    public virtual void  Start () {
		
	}
    // Update is called once per frame
    public virtual void Update ()
    {
		
	}
    public virtual void FixedUpdate()
    {

    }
    public virtual void OnDestroy()
    {

    }
}
