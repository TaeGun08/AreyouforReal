using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    
    [SerializeField] private TMP_Text roomCode;
    [SerializeField] private Button startButton;
    [SerializeField] private TMP_InputField chatInput;
    [SerializeField] private Button chatSendButton;
    
    private NetworkRunner runner;

    private void OnEnable()
    {
        startButton.onClick.AddListener(() =>
        {
            PlayerRegistry.Instance.MovePlayer(new Vector3(100, 2, 0));
            Debug.Log("게임시작버튼 클릭!");
        });
        
        chatSendButton.onClick.AddListener(() =>
        {
            ChattingSystem.Instance.SendChat(chatInput.text, "asd");
            chatInput.text = string.Empty;
        });
    }

    private void Start()
    {
        runner = FindObjectOfType<NetworkRunner>();
        roomCode.SetText(runner.SessionInfo.Name);
    }
}
