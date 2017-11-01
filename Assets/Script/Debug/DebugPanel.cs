using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPanel : MonoBehaviour {

    PlayerBrain playerBrain;
	void Start () {
        playerBrain = FindObjectOfType<PlayerBrain>();

    }
    void Possession()
    {
        RaycastHit hit;
        GameObject lastPuppet;
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.position + ray.direction,Color.red,0.5f);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Puppet puppet = hit.transform.GetComponent<Puppet>();
                if (puppet != null)
                {
                    Debug.Log("Possesion !!!");
                    if (playerBrain != null)
                    {
                        lastPuppet = playerBrain.gameObject;
                        Destroy(playerBrain);
                        lastPuppet.AddComponent<MouseBrain>();
                    }
                    playerBrain = puppet.gameObject.AddComponent<PlayerBrain>();
                    Destroy(puppet.GetComponent<MouseBrain>());
                }
            }
        }
    }
	// Update is called once per frame
	void Update () {
        Possession();

    }
}
