using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook camera;
    
    private IEnumerator Start()
    {
        while (Player.LocalPlayer == null)
            yield return null;
        
        camera.Follow = Player.LocalPlayer.transform;
        camera.LookAt = Player.LocalPlayer.transform;
    }
}
