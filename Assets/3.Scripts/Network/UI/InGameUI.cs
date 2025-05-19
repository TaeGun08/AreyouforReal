using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Fusion;
using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text roomCode;
    private NetworkRunner runner;
    
    private void Start()
    {
        runner = FindObjectOfType<NetworkRunner>();
        roomCode.SetText(runner.SessionInfo.Name);
    }
}
