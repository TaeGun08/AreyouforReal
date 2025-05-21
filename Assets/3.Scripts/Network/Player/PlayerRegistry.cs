using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerRegistry : NetworkBehaviour
        , INetworkRunnerCallbacks
    {
        public static PlayerRegistry Instance { get; private set; }
        
        public const byte CAPACITY = 8;
        
        // 패킷으로 보내기 위해서 Capacity를 사용해야함
        // 왜? 자료구조들은 가변이라 프로토콜에 맞지 않아서
        // UnitySerializeField인스펙터에서 보이게
        [Networked, Capacity(CAPACITY)]
        [UnitySerializeField]
        private NetworkDictionary<PlayerRef, LocalPlayer> playerDic { get; }

        public override void Spawned()
        {
            Instance = this;
            Runner.AddCallbacks(this);
        }
        
        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            Instance = null;
            runner.RemoveCallbacks(this);
        }

        public LocalPlayer GetPlayerOrNull(PlayerRef pRef)
        {
            if (playerDic.ContainsKey(pRef))
            {
                return playerDic[pRef];
            }
            
            return null;
        }

        public void MovePlayer(Vector3 position)
        {
            // 서버용 호출인데 서버가 아닌곳에서 호출하면 에러! 디버그때만 잡힘
            Debug.Assert(Runner.IsServer);
            
            foreach (var player in playerDic)
            {
                // 캐싱필요
                player.Value.GetComponent<NetworkCharacterController>().Teleport(position);
            }
            // TODO : 플레이이동
        }
        
        /// <summary>
        /// 호스트만 Add하지만 [Network]로 동기화해서 모두 공유함!
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="pRef"></param>
        /// <param name="playerObject"></param>
        public void AddPlayer(NetworkRunner runner, PlayerRef pRef, LocalPlayer localPlayer)
        {
            // 서버용 호출인데 서버가 아닌곳에서 호출하면 에러! 디버그때만 잡힘
            Debug.Assert(runner.IsServer);
            
            // TODO : 유효성 검사
            playerDic.Add(pRef, localPlayer);

            // foreach (var p in playerDic)
            // {
            //     Debug.Log($"Player : {p.Key.PlayerId}");
            // }
        }
        
        public void RemovePlayer(NetworkRunner runner, PlayerRef pRef)
        {
            // 서버용 호출인데 서버가 아닌곳에서 호출하면 에러! 디버그때만 잡힘
            Debug.Assert(runner.IsServer);

            if (playerDic.Remove(pRef) == false)
            {
                Debug.LogWarning("dic에 플레이어 없음");
            }
        }
        
        // Host만 처리
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                RemovePlayer(runner, player);
            }
        }
        
        #region INetworkRunnerCallbacks
        
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
        public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, Fusion.Sockets.NetAddress remoteAddress, Fusion.Sockets.NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, System.ArraySegment<byte> data) { }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
        
        #endregion
    }
}