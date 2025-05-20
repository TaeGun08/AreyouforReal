using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Fusion;
using Fusion.Sockets;
using UnityEngine;


// 인풋처리
public class PlayerInputBehaviour : SimulationBehaviour
    , INetworkRunnerCallbacks
{
    public static PlayerInputBehaviour Instance { get; private set; }
    private bool attackButton;

    private Joystick joystick;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        attackButton |= Input.GetMouseButton(0);
    }

    private void LateUpdate()
    {
        if (joystick == null)
        {
            joystick = Joystick.Instance;
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }
    
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData data = new NetworkInputData();

        if (joystick == null) return; 
        data.Horizontal += joystick.Horizontal;
        data.Vertical += joystick.Vertical;
        
        input.Set(data);
    }
    
    #region INetworkRunnerCallbacks
    
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }
    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }
    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }
    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }
    
    #endregion 안씀
}
