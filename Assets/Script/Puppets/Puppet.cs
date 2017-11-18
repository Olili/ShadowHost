using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Steering))]
public class Puppet : MonoBehaviour {

    static readonly float maxSlope = 45;

    [SerializeField]private CreatureType type;
    public Stats stats = new Stats();
    [SerializeField]private Puppet leader;
    [SerializeField] private HordeManager hordeManager;
    private Vector3 extents;
    private float life;
    public string debugAction;
    [SerializeField] private bool isOnGround;
    [HideInInspector] public Vector3 OnPlanNormal;
    public bool slidingDebug = false;
    Transform centerDown;

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
            {
                puppetAction.OnBegin();
                debugAction = puppetAction.ToString();
            }
        }
    }
    public bool IsOnGround
    {
        get
        {
            return isOnGround;
        }
        set
        {
            if (value == true)
                Rb.useGravity = false;
            else
                Rb.useGravity = true;
            isOnGround = value;
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

    public HordeManager HordeManager
    {
        get
        {
            return hordeManager;
        }

        set
        {
            hordeManager = value;
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
        OnPlanNormal = Vector3.up;
        PhysicMaterial.dynamicFriction = 0;
        centerDown = transform.Find("CenterDown");

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
        HordeManager = parent.GetComponent<HordeManager>();
    }
    protected virtual void GroundGravityCheck()
    {
        int mask = LayerMask.GetMask(new string[] { "Default"});
        RaycastHit hit;
        Vector3 center = centerDown.position + Vector3.up * Extents.y;
      
        if (Physics.Raycast(center, -OnPlanNormal, out hit, (Extents.y +0.5f), mask))
        {
            slidingDebug = false;
                // On est dans une pente il faut tomber
            if (Vector3.Angle(Vector3.up, hit.normal) > maxSlope)
            {
                Slide();
                slidingDebug = true;
                IsOnGround = false;
            }
                // On est proche du sol mais pas assez
            else if (Vector3.Distance(hit.point, centerDown.position) > 0.2f)
            {
                rb.AddForce(Physics.gravity*10, ForceMode.Acceleration);
                IsOnGround = true;
            }
            else
            {
                IsOnGround = true;
            }
            OnPlanNormal = hit.normal;
        }
        else // Raycast failure : on est loin du sol
        {
            IsOnGround = false;
        }
    }
    private void Slide()
    {
        Vector3 right = Vector3.Cross(OnPlanNormal, Vector3.up);
        //Vector3 downSlide = Vector3.Cross(right, OnPlanNormal);
        Vector3 downSlide = Vector3.Cross(OnPlanNormal, right);
        Rb.AddForce(downSlide.normalized * 10, ForceMode.Acceleration);
    }
   
    public Vector3 GetVelocity()
    {
        return Rb.velocity;
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
        GroundGravityCheck();
        if (!IsOnGround)
            puppetAction.RotateTowardGround();
        if (puppetAction != null)
            puppetAction.OnFixedUpdate();
        if (transform.position.y < -1000)
        {
            Debug.Log("Out of map " + gameObject.name);
            puppetAction.OnDeath();
        }
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
    public void OnTriggerEnter(Collider other)
    {
        if (puppetAction != null)
            puppetAction.TriggerEnter(other);
    }
    public  void OnTriggerStay(Collider other)
    {
        if (puppetAction != null)
            puppetAction.TriggerStay(other);
    }
    public void OnTriggerExit(Collider other)
    {
        if (puppetAction != null)
            puppetAction.TriggerExit(other);
    }
}
