using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fourre tout peut être amélioré.

public class DebugPanel : MonoBehaviour {

    PlayerBrain playerBrain;
    HordeCreator hordeCreator;
    [Range(0, 1)]  public float timeScale = 1;

	void Start () {
        playerBrain = FindObjectOfType<PlayerBrain>();
        hordeCreator = FindObjectOfType<HordeCreator>();
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
                    GameManager.Instance.Possession(puppet);
                    //Debug.Log("Possesion !!!");
                }
            }
        }
    }
    void PopHordDebug()
    {
      
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            hordeCreator.CreateDeadPuppet(CreatureType.Spider);
        }
        else if (Input.GetKey(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            hordeCreator.CreateHorde(Vector3.zero, CreatureType.Spider, 10);
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.Alpha3))
        {
            hordeCreator.CreateDeadPuppet(CreatureType.Grunt);
        }
        else if (Input.GetKey(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.Alpha3))
        {
            hordeCreator.CreateHorde(Vector3.zero, CreatureType.Grunt, 4);
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
        PopHordDebug();
    }
}
