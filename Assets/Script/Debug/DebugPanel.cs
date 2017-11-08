using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fourre tout peut être amélioré.

public class DebugPanel : MonoBehaviour {

    PlayerBrain playerBrain;
    [Range(0, 1)]  public float timeScale = 1;

	void Start () {
        playerBrain = FindObjectOfType<PlayerBrain>();

    }

    // ne marche plus. temporaire.
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
                        lastPuppet.AddComponent<FollowBrain>();
                    }
                    playerBrain = puppet.gameObject.AddComponent<PlayerBrain>();
                    Destroy(puppet.GetComponent<FollowBrain>());
                }
            }
        }
    }
    void UpdateFrameRate()
    {
        if (Time.timeScale != timeScale)
            Time.timeScale = timeScale;
    }
	// Update is called once per frame
	void Update () {
        Possession();
        UpdateFrameRate();
    }
}
