using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private TMP_InputField codeText;
    
    private void OnEnable()
    {
        hostButton.onClick.AddListener(() => {
            // 코드 자동생성
            _ = NetworkStartBridge.Instance.StartGame(GameMode.Host);
        });
        
        joinButton.onClick.AddListener(() => 
        {
            // SetCode를 통해 Join 
            NetworkStartBridge.Instance.SetCode(codeText.text);
            _ = NetworkStartBridge.Instance.StartGame(GameMode.Client);
        });
        
        
    }
}
