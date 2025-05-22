using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Fusion;
using TMPro;
using UnityEngine;

public class ChattingSystem : NetworkBehaviour
{
    public static ChattingSystem Instance { get; private set; }
    
    [SerializeField] private GameObject chatAreaGameObject;
    [SerializeField] private GameObject chatPrefab;
    
    
    // private List<(string, string)> messageList = new List<(string, string)>();
    
    private void Awake()
    {
        Instance = this;
    }

    // public void SendChat(string message, string sender)
    // {
    //     RPC_SendChat(message, sender);
    // }
    //
    // [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    // public void RPC_SendChat(string message, string sender)
    // {
    //     RPC_ReceiveChat(message, sender);
    // }
    
    public void RPC_SendChat(string message, string sender)
    {
        RPC_ReceiveChat(message, sender);
    }
    
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_ReceiveChat(string message, string sender)
    {
        Debug.Log("ReceiveChat : " + message);
        
        GameObject chat = Instantiate(chatPrefab, parent: chatAreaGameObject.transform);
        chat.GetComponentInChildren<TMP_Text>().SetText($"{sender}: {message}");
        
        // messageList.Add((message, sender));
    }
}
