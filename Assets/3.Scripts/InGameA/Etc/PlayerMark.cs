using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMark : MonoBehaviour
{
    private Player localPlayer;
    
    private IEnumerator Start()
    {
        while (Player.LocalPlayer == null)
            yield return null;
        
        localPlayer = Player.LocalPlayer;
    }

    private void LateUpdate()
    {
        transform.position = localPlayer.transform.position;
    }
}
