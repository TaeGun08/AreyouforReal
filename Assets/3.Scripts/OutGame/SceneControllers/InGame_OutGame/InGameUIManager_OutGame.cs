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
    
    [Space]
    [Header("PopUps")]
    [SerializeField] private GameObject Popup_Chat;
    
    [Space]
    [Header("PopUps")]
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject waitingButton;
    
    private NetworkRunner runner;
    
    public static InGameUIManager_OutGame Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        startButton.SetActive(false);
        waitingButton.SetActive(false);
        runner = FindObjectOfType<NetworkRunner>();
        roomCode.SetText(runner.SessionInfo.Name);
    }

    public void OnClickedStartButton()
    {
        bool isStart = GameManager_Network.Instance.TryStartGame();
        
        if (isStart)
        {
            startButton.gameObject.SetActive(false);
        }
    }
    
    public void OnClickedChatButton()
    {
        Popup_Chat.SetActive(true);
    }
    
    public void UpdateButtonState(int playerCount)  //시작버튼 활성화
    {
        if(!runner.IsServer) return;  //서버 아니면 날림
        
        if (playerCount < 2)
        {
            startButton.SetActive(false);
            waitingButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(true);
            waitingButton.SetActive(false);
        }
    }
}
