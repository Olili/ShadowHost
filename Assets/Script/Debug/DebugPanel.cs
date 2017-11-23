using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fourre tout peut être amélioré.

public class DebugPanel : MonoBehaviour {

    PlayerBrain playerBrain;
    HordeCreator hordeCreator;
    [Range(0, 15)]  public float timeScale = 1;

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
        // Spider corpse
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                hordeCreator.CreateDeadPuppet(CreatureType.Spider, hit.point);
            }

        }        // Spider horde
        else if (Input.GetKey(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                hordeCreator.CreateHorde(hit.point, CreatureType.Spider, 1);
            }

        }   // Grunt horde
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.Alpha3))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                hordeCreator.CreateDeadPuppet(CreatureType.Grunt, hit.point);
            }
        }
        else if (Input.GetKey(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.Alpha3))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                hordeCreator.CreateHorde(hit.point, CreatureType.Grunt, 3);

            }
        }
            // Humain horde
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.Alpha4))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                hordeCreator.CreateDeadPuppet(CreatureType.Humain, hit.point);
            }
        }
        // Humain corpse
        else if (Input.GetKey(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.Alpha4))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                hordeCreator.CreateHorde(hit.point, CreatureType.Humain, 1);

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
        PopHordDebug();


        if (Input.GetKeyDown(KeyCode.L))
        {
            GameManager.Instance.PlayerBrain.puppet.PuppetAction.OnDeath();
        }
    }
}
