using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private PlayerBrain playerBrain;
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<GameManager>();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }

        private set { }
    }
    public void Possession(Puppet puppet)
    {
        Brain brain = puppet.GetComponent<Brain>();


        if (brain == playerBrain && playerBrain != null)
        {
            Debug.Log("Unit déjà possedee");
            return;
        }

        if (playerBrain != null)
        {
            Destroy(playerBrain);
        }
        switch (puppet.Type)
        {
            case CreatureType.Spider:
                playerBrain = puppet.gameObject.AddComponent<PlayerBrain>();
                break;
            case CreatureType.Grunt:
                playerBrain = puppet.gameObject.AddComponent<PlayerGruntBrain>();
                break;
            case CreatureType.Max_Creatures:
                Debug.Log("What it is ?");
                break;
            default:
                break;
        }

    }
}
