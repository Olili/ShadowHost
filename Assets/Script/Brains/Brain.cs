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
        chase,
        alphaFight
    }
    public virtual IA_State GetTypeState(E_State _state)
    {
        Puppet _myPuppet = puppet;
        CreatureType _type = puppet.Type;
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
                    default:
                        return new Follow_State(_myPuppet);
                }
            }
            else if (_state == E_State.chase)
            {
                switch (_type)
                {
                    case CreatureType.Spider:
                        return new ChaseSpider_State(_myPuppet);
                        break;

                    case CreatureType.Grunt:
                        return new ChaseGrunt_State(_myPuppet);
                        break;
                    default:
                        return new Follow_State(_myPuppet);
                }
            }
            else if (_state == E_State.alphaFight)
            {
                switch (_type)
                {
                    case CreatureType.Spider:
                        return new CircleLeaders_state(_myPuppet);
                        break;

                    case CreatureType.Grunt:
                        return new CircleLeaders_state(_myPuppet);
                        break;
                    default:
                        return new CircleLeaders_state(_myPuppet);
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
                        return new AlphaGuide_State(_myPuppet);

                    case E_State.chase:
                        return new ChaseSpider_State(_myPuppet);
                    case E_State.alphaFight:
                        return new SpiderAlphasFight_State(_myPuppet);
                        break;
                    default:
                        return new AlphaGuide_State(_myPuppet);
                }
            }
            else if (_type == CreatureType.Grunt)
            {
                switch (_state)
                {
                    case E_State.follow:
                        return new AlphaGuide_State(_myPuppet);
                        break;
                    case E_State.chase:
                        return new ChaseGrunt_State(_myPuppet);
                        break;
                    case E_State.alphaFight:
                        return new GruntAlphasFight_State(_myPuppet);
                        break;
                    default:
                        return new AlphaGuide_State(_myPuppet); 
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
