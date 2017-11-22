using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour {

    private CinemachineTransposer transposer;
    private CinemachineVirtualCamera cineVC;
    public float FollowOffsetForward;
    public void Awake()
    {
        cineVC = GetComponentInChildren<CinemachineVirtualCamera>();
        transposer = cineVC.GetComponent<CinemachineTransposer>();
    }
    
    void Update()
    {
    //    transposer.m_FollowOffset = transform.forward * FollowOffsetForward;
    }

    public void SetTarget(Transform _transform)
    {
        cineVC.Follow = (_transform);
    }

  
}
