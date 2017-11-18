using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour {

	// Use this for initialization

    private void Awake()
    {
        GameManager.Instance.InterfaceManager = this;
    }
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}




	public void HighlightDeadPuppet(Puppet p)
	{
		
	}
}
