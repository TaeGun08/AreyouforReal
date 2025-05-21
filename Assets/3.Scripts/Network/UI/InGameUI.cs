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
    
    private NetworkRunner runner;

    private void OnEnable()
    {
        startButton.onClick.AddListener(() =>
        {
            PlayerRegistry.Instance.MovePlayer(new Vector3(100, 2, 0));
            Debug.Log("게임시작버튼 클릭!");
        });
    }

    private void Start()
    {
        runner = FindObjectOfType<NetworkRunner>();
        roomCode.SetText(runner.SessionInfo.Name);
    }
}
