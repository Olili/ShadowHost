using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Steering))]
public class Puppet : MonoBehaviour {

    [SerializeField]private CreatureType type;
    public Stats stats = new Stats();
    private Puppet leader;
    private Vector3 extents;
    private float life;

        // State machine : 
    private PuppetAction puppetAction;

        // unityStuff
    private PhysicMaterial physicMaterial;
    private Rigidbody rb;
    private Animator animator;
    private Material material;

#region GetterSetters
    public Vector3 Extents
    {
        get{return extents;}
    }

    public PuppetAction PuppetAction
    {
        get
        {
            return puppetAction;
        }

        set
        {
            if (puppetAction != null)
                puppetAction.OnEnd();
            puppetAction = value;
            if (puppetAction!=null)
                puppetAction.OnBegin();
        }
    }

    public Animator Animator
    {
        get
        {
            return animator;
        }

        set
        {
            animator = value;
        }
    }

    public PhysicMaterial PhysicMaterial
    {
        get
        {
            return physicMaterial;
        }

        set
        {
            physicMaterial = value;
        }
    }

    public Rigidbody Rb
    {
        get
        {
            return rb;
        }

        set
        {
            rb = value;
        }
    }

    public Puppet Leader
    {
        get
        {
            return leader;
        }

        set
        {
            leader = value;
        }
    }

    public CreatureType Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }

    public float Life
    {
        get
        {
            return life;
        }

        set
        {
            life = value;
        }
    }
    #endregion

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        PhysicMaterial = GetComponent<Collider>().material;
        stats.Init();
        extents = GetComponent<Collider>().bounds.extents;
        Life = stats.Get(Stats.StatType.maxLife);
        switch (Type)
        {
            case CreatureType.Spider:
                PuppetAction = new SpiderAction(this);  
                break;
            case CreatureType.Grunt:
                PuppetAction = new GruntAction(this);
                break;
            case CreatureType.Max_Creatures:
                Debug.Log("Error : creatureType");
                break;
            default:
                break;
        }
    }
    public void Init(Vector3 position,Puppet _leader, Transform parent)
    {
        gameObject.SetActive(true);
        transform.position = position;
        transform.parent = parent;
        Leader = _leader;
    }
    //public virtual void Move(Vector3 direction, float factor)
    //{
    //    Vector3 translation = direction * 1 * stats.Get(Stats.StatType.move_speed) * Time.fixedDeltaTime;
    //    rb.MovePosition(rb.transform.position + translation);
    //}
   
    public Vector3 GetVelocity()
    {
        return Rb.velocity;
    }


    public virtual void OnTakeDamage()
    {

    }
    public void ChangeColorDebug()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.red;
    }
    public void Update()
    {
        if (puppetAction != null)
            puppetAction.OnUpdate();
    }
    public void FixedUpdate()
    {
        if (puppetAction != null)
            puppetAction.OnFixedUpdate();
    }
    public void OnDrawGizmos()
    {
        if (puppetAction!=null)
            puppetAction.DrawGizmo();
    }

    public void OnAnimationEvent()
    {
        puppetAction.OnAnimationEvent();
    }
}
