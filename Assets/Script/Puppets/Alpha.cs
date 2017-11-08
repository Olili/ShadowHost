using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Alpha : MonoBehaviour
{
    List<Puppet> hordePuppets = new List<Puppet>();
    Puppet alphaPuppet;
    float timer = 0;
    private void Awake()
    {
        alphaPuppet = GetComponent<Puppet>();
    }
    public void AddHordePuppet(Puppet puppet)
    {
        hordePuppets.Add(puppet);
    }
    public void RemoveHordePuppet(Puppet puppet)
    {
        hordePuppets.Remove(puppet);
    }
   
    public void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer> 0.5f)
        {
            CircleChill();
            timer = 0;
        }
    }

    // Dessine un cercle imaginaire autour du joueur. 
    // On regarde combien d'unités sont dans ce cercle.
    // S'il y a en assez on leur demande d'arrêter d'avancer vers le joueur.
    // On recommence en augmentant la taille du cercle.
    void CircleChill()
    {
        float creatureRay = alphaPuppet.Extents.magnitude * 1.2f;
        float unitArea = (Mathf.PI * creatureRay * creatureRay);
        float totalHordeArea = unitArea * hordePuppets.Count;
        float HordeMaxRay = Mathf.Sqrt(totalHordeArea / Mathf.PI);

        //Debug.Log("HordeMaxRay :" + HordeMaxRay);
        List<Puppet> puppetNearby = new List<Puppet>();
        float baseRay = alphaPuppet.Extents.magnitude * 2;

        for (; baseRay < HordeMaxRay; baseRay += (alphaPuppet.Extents.magnitude*2))
        {
            CircleCheck(baseRay, puppetNearby,unitArea);
        }
        CircleCheck(HordeMaxRay, puppetNearby, unitArea);

        maxStuffSize = HordeMaxRay; // debugGizmos
    }
    void CircleCheck(float maxDistance, List<Puppet> puppetNearby, float unitVolume)
    {
        for (int i = 0; i < hordePuppets.Count;i++)
        {
            if (!puppetNearby.Contains(hordePuppets[i]))
            {
                float distance = Vector3.Distance(transform.position, hordePuppets[i].transform.position);
                if (distance <= maxDistance)
                    puppetNearby.Add(hordePuppets[i]);
            }
        }
        float circleVolume = Mathf.PI * maxDistance * maxDistance;
        int puppetNbInArea = Mathf.FloorToInt((circleVolume / unitVolume));

        if (puppetNearby.Count >= puppetNbInArea)
        {
            for (int i = 0; i < puppetNearby.Count;i++)
            {
                FollowBrain followBrain = puppetNearby[i].GetComponent<FollowBrain>();
                followBrain.chillTest = true;
            }
            //Debug.Log("Chill circle  " + maxDistance);
        }
    }
    float maxStuffSize = 0;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, maxStuffSize);
    }
}