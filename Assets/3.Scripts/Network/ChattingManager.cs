using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ChattingSystem : MonoBehaviour
{
    public static ChattingSystem Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }

    // [Rpc(RpcSources.InputAuthority, RpcTargets.InputAuthority)]
    // public void RPC_SendChat(string message)
    // {
    //     RPC_ReceiveChat(message);
    // }

    public void SendChat(string message)
    {
        RPC_ReceiveChat(message);
    }
    
    [Rpc(RpcSources.InputAuthority, RpcTargets.InputAuthority)]
    public void RPC_ReceiveChat(string message)
    {
        Debug.Log(message);
    }
}
