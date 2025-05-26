using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager_OutGame : MonoBehaviour
{
    [Header("Top RoomCode UI")]
    [SerializeField] private TMP_Text roomCode;
    
    [Header("PopUps")]
    [SerializeField] private GameObject Popup_Chat;
    
    private NetworkRunner runner;
    
    private void Start()
    {
        runner = FindObjectOfType<NetworkRunner>();
        roomCode.SetText(runner.SessionInfo.Name);
    }

    public void OnClickedStartButton()
    {
        PlayerRegistry.Instance.MovePlayer(new Vector3(100, 2, 0));
    }
    
    public void OnClickedChatButton()
    {
        Popup_Chat.SetActive(true);
    }
}
