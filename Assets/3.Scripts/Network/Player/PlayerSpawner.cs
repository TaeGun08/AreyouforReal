using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Fusion;
using Fusion.Addons.Physics;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

// 스포너 스폰역할만
public class PlayerSpawner : MonoBehaviour
    , INetworkRunnerCallbacks
{
    [Header("References")]
    [SerializeField] private NetworkPrefabRef playerPrefab;
    
    // Host만 처리
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Vector3 spawnPosition = new Vector3(0, 2, 0);
            runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity
                , inputAuthority: player);
        }
        
        Debug.Log("Player joined");
    }
    
    // Host만 처리
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            LocalPlayer leftPlayer = PlayerRegistry.Instance.GetPlayerOrNull(player);
            
            if (leftPlayer != null)
            {
                runner.Despawn(leftPlayer.Object);
            }
        }
    }
    
    #region INetworkRunnerCallbacks
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
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
    public void OnInput(NetworkRunner runner, NetworkInput input)
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
    #endregion INetworkRunnerCallbacks
}
