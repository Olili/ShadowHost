using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    public  Puppet puppet;

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


    #region IA_STATE_Getter
    public enum E_State
    {
        free,
        follow,
        circle,
        chase,
        fight
    }
    public virtual IA_State GetTypeState(E_State _state)
    {
        Puppet _myPuppet = puppet;
        CreatureType _type = puppet.Type;

        if (puppet.Leader == null)
            Debug.LogError("Leader must be set to give good state");
        bool iAmAnAplha = puppet.Leader == puppet;

        if (!iAmAnAplha)
        {
            if (_state == E_State.follow)
            {
                switch (_type)
                {
                    case CreatureType.Spider:
                        return new SpiderFollow_State(_myPuppet);

                    case CreatureType.Grunt:
                        return new GruntFollow_State(_myPuppet);
                    case CreatureType.Humain:

                    default:
                        return new Follow_State(_myPuppet);
                }
            }
            else if (_state == E_State.circle)
            {
                switch (_type)
                {
                    case CreatureType.Spider:
                        return new CircleLeaders_state(_myPuppet);
                    case CreatureType.Grunt:
                        return new CircleLeaders_state(_myPuppet);
                    case CreatureType.Humain:
                        return new CircleLeaders_state(_myPuppet);
                    default:
                        return new CircleLeaders_state(_myPuppet);
                }
            }
            else if (_state == E_State.chase)
            {
                switch (_type)
                {
                    default:
                        return new Chase_State(_myPuppet);
                }
            }
            else if (_state == E_State.fight)
            {
                switch (_type)
                {
                    default:
                        return new Fight_State(_myPuppet);
                }
            }
        }
        else// si je suis alpha
        {
            if (_state == E_State.follow)
            {
                switch (_type)
                {
                    default:
                        return new AlphaGuide_State(_myPuppet);
                }
            }
            else if (_state == E_State.chase)
            {
                switch (_type)
                {
                    default:
                        return new Chase_State(_myPuppet);
                }
            }
            else if (_state == E_State.fight)
            {
                switch (_type)
                {
                    default:
                        return new Fight_State(_myPuppet);
                }
            }
        }
        return null;
    }
    #endregion

    /*


    */


}
