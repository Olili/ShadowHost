using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    protected Puppet puppet;

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
        follow,
        chase
    }
    public virtual IA_State GetTypeState(Puppet _myPuppet, E_State _state, CreatureType _type, bool iAmAnAplha = false, Puppet _myFoePuppet = null)
    {
        if (!iAmAnAplha)
        {
            if (_state == E_State.follow)
            {
                switch (_type)
                {
                    case CreatureType.Spider:


                    case CreatureType.Grunt:

                    default:
                        return new Follow_State(_myPuppet);
                }
            }
            else if (_state == E_State.chase)
            {
                switch (_type)
                {
                    //case CreatureType.Spider:
                    //    return new ChaseSpider_State()
                    //    break;

                    case CreatureType.Grunt:
                        break;
                    default:
                        return new Follow_State(_myPuppet);
                }
            }
        }
        else
        {
            if (_type == CreatureType.Spider)
            {
                switch (_state)
                {
                    case E_State.follow:


                    case E_State.chase:

                    default:
                        return new Follow_State(_myPuppet);
                }
            }
            else if (_type == CreatureType.Grunt)
            {
                switch (_state)
                {
                    case E_State.follow:
                        return new Follow_State(_myPuppet);
                        break;
                    case E_State.chase:
                        break;
                    default:
                        break;
                }
            }

        }
        return null;
    }
    #endregion

    /*


    */


}
