using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrain : Brain {

    Rigidbody rb;
    StatBuff turnSpeedBuff;
    StatBuff speedBuff;
    public Host host;
    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }
    public override void Start () {
        base.Start();
        turnSpeedBuff = new StatBuff(Stats.StatType.maxTurnSpeed, 500, -1);
        speedBuff = new StatBuff(Stats.StatType.move_speed, 1.5f, -1);
        puppet.stats.AddBuff(turnSpeedBuff);
        puppet.stats.AddBuff(speedBuff);
        puppet.Leader = puppet;
        GameManager.Instance.PlayerBrain = this;
    }
    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.P) && GetComponent<Host>() == false)
        {
            puppet.PuppetAction.OnDeath();
        }
    }

    public virtual void Move()
    {
        Vector3 direction = Vector3.zero;
        direction += Camera.main.transform.forward * Input.GetAxis("Vertical");
        direction += Camera.main.transform.right * Input.GetAxis("Horizontal");
        direction.y = 0;
        direction.Normalize();
        direction = direction * puppet.stats.Get(Stats.StatType.move_speed);

        direction.y = puppet.Rb.velocity.y;

        if (puppet.IsOnGround == true)
            puppet.PuppetAction.SetVelocity(direction);

        if (direction.x != 0 || direction.z != 0)
        {
            //direction = GetComponent<Steering>().GetDvOnPlan(transform.position + direction);
            puppet.PuppetAction.SetRotation(direction.normalized);
        }

    }

    public override void FixedUpdate () {

        base.FixedUpdate();
        Move();
        CircleChill();
    }
    public override void OnDestroy()
    {
        puppet.stats.RemoveBuff(turnSpeedBuff);
        puppet.stats.RemoveBuff(speedBuff);
    }



    // Dessine un cercle imaginaire autour du joueur. 
    // On regarde combien d'unités sont dans ce cercle.
    // S'il y a en assez on leur demande d'arrêter d'avancer vers le joueur.
    // On recommence en augmentant la taille du cercle.
    void CircleChill()
    {
        if (GetComponent<Host>() != null)
            return;
        if (puppet.Rb.velocity.magnitude > 0.2f)
            return;
        if (puppet.HordeManager.HordePuppets.Count < 2)
            return;

            float creatureRay = puppet.Extents.magnitude * 1.2f;
        float unitArea = (Mathf.PI * creatureRay * creatureRay);
        float totalHordeArea = unitArea * puppet.HordeManager.HordePuppets.Count;
        float HordeMaxRay = Mathf.Sqrt(totalHordeArea / Mathf.PI);

        //Debug.Log("HordeMaxRay :" + HordeMaxRay);
        List<Puppet> puppetNearby = new List<Puppet>();
        float baseRay = puppet.Extents.magnitude * 2;

        for (; baseRay < HordeMaxRay; baseRay += (puppet.Extents.magnitude * 2))
        {
            CircleCheck(baseRay, puppetNearby, unitArea);
        }
        CircleCheck(HordeMaxRay, puppetNearby, unitArea);

    }
    void CircleCheck(float maxDistance, List<Puppet> puppetNearby, float unitVolume)
    {

        for (int i = 0; i < puppet.HordeManager.HordePuppets.Count; i++)
        {
            if (!puppetNearby.Contains(puppet.HordeManager.HordePuppets[i])
                && puppet.HordeManager.HordePuppets[i] != null && puppet.HordeManager.HordePuppets[i] != puppet)
            {
                float distance = Vector3.Distance(puppet.transform.position, puppet.HordeManager.HordePuppets[i].transform.position);
                if (distance <= maxDistance)
                    puppetNearby.Add(puppet.HordeManager.HordePuppets[i]);
            }
        }
        float circleVolume = Mathf.PI * maxDistance * maxDistance;
        int puppetNbInArea = Mathf.FloorToInt((circleVolume / unitVolume));

        if (puppetNearby.Count >= puppetNbInArea)
        {
            for (int i = 0; i < puppetNearby.Count; i++)
            {
                IA_Brain iaBrain = puppetNearby[i].GetComponent<IA_Brain>();
                if (iaBrain != null && iaBrain.MyIAState is Follow_State)
                {
                    (iaBrain.MyIAState as Follow_State).chillTest = true;
                }

            }
        }
    }
}
