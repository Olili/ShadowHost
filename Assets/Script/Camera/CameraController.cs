using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour {

    private CinemachineVirtualCamera cineVC;

    public void Awake()
    {
        cineVC = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    public void SetTarget(Transform transform)
    {
        cineVC.Follow = GameManager.Instance.PlayerBrain.transform;
    }
  
}
