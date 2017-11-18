using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyParticleSystem : MonoBehaviour {

	// Use this for initialization
	ParticleSystem particleSystem;
	void Start () {
		particleSystem = GetComponent<ParticleSystem>();
	//	Invoke("AutoDestroy", particleSystem.main.duration + particleSystem.main.startLifetime.constant);
	}

	void AutoDestroy()
	{
		Destroy(gameObject);
	}
	
}
