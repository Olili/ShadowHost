using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour {

	// Use this for initialization

    private void Awake()
    {
        GameManager.Instance.interfaceManager = this;
    }
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
