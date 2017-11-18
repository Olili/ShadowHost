using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private PlayerBrain playerBrain;
    public  HordeCreator hordeCreator;

    private InterfaceManager interfaceManager;
    private static GameManager instance = null;

#region getterSetters
    public PlayerBrain PlayerBrain
    {
        get
        {
            return playerBrain;
        }
        set
        {
            playerBrain = value;
            if (playerBrain!= null)
            {
                CameraController cameraController = Camera.main.GetComponentInParent<CameraController>();
                if (cameraController!=null)
                    cameraController.SetTarget(playerBrain.transform);
            }
        }
    }


    public InterfaceManager InterfaceManager
    {
        get
        {
            if(interfaceManager == null)
            {
                interfaceManager = new GameObject("InterfaceManager").AddComponent<InterfaceManager>();
            }
            return interfaceManager;
        }
        set
        {
            interfaceManager = value;
        }
    }
#endregion

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

        if (brain == PlayerBrain && PlayerBrain != null)
        {
            Debug.Log("Unit déjà possedee");
            return;
        }
        if (PlayerBrain != null)
        {
            Destroy(PlayerBrain);
        }
        switch (puppet.Type)
        {
            case CreatureType.Spider:
                puppet.gameObject.AddComponent<PlayerBrain>();
                break;
            case CreatureType.Grunt:
                puppet.gameObject.AddComponent<PlayerGruntBrain>();
                break;
            case CreatureType.Max_Creatures:
                Debug.Log("What it is ?");
                break;
            default:
                break;
        }
        puppet.Life = puppet.stats.Get(Stats.StatType.maxLife);
    }
}
