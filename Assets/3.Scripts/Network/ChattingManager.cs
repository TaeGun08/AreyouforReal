using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ChattingSystem : MonoBehaviour
{
    public static ChattingSystem Instance { get; private set; }
    private List<(string, string)> messageList = new List<(string, string)>();
    
    private void Awake()
    {
        Instance = this;
    }

    // [Rpc(RpcSources.InputAuthority, RpcTargets.InputAuthority)]
    // public void RPC_SendChat(string message)
    // {
    //     RPC_ReceiveChat(message);
    // }

    public void SendChat(string message, string sender)
    {
        RPC_ReceiveChat(message, sender);
    }
    
    [Rpc(RpcSources.InputAuthority, RpcTargets.InputAuthority)]
    public void RPC_ReceiveChat(string message, string sender)
    {
        Debug.Log("ReceiveChat : " + message);
        messageList.Add((message, sender));

        foreach (var props in messageList)
        {
            Debug.Log(props.Item1);
        }
    }
}
