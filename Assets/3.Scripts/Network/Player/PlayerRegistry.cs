using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        public NetworkDictionary<PlayerRef, LocalPlayer> playerDic { get; }

        [SerializeField] private int index;
        [SerializeField] private TMP_Text countText;
        [Networked] private string CountString { get; set; }
        
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

            if (playerDic.Add(pRef, localPlayer))
            {
                RPC_Count();
                
                var userCount = new Dictionary<string, object>
                {
                    { "MembersCount", playerDic.Count }
                };

                if (GameManager_Network.Instance.State == GameManager_Network.GameState.Wait)
                {
                    InGameUIManager_OutGame.Instance.UpdateButtonState(index <= playerDic.Count);
                }
                
                _ = FirestoreManager.Instance.UpdateDataAsync(
                    FirebaseCollections.Rooms, runner.SessionInfo.Properties["RoomId"], userCount);
                
                // Debug.Log("업데이트");
            }
            else
            {
                Debug.LogWarning("PlayerRegistry 추가 못함!");
            }
        }
        
        [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
        private void RPC_Count()
        {
            CountString = $"현재 인원: {playerDic.Count}";
            countText.text = CountString;
        }
        
        public void RemovePlayer(NetworkRunner runner, PlayerRef pRef)
        {
            // 서버용 호출인데 서버가 아닌곳에서 호출하면 에러! 디버그때만 잡힘
            Debug.Assert(runner.IsServer);

            if (playerDic.Remove(pRef))
            {
                var userCount = new Dictionary<string, object>
                {
                    { "MembersCount", playerDic.Count }
                };
                
                if (GameManager_Network.Instance.State == GameManager_Network.GameState.Wait)
                {
                    InGameUIManager_OutGame.Instance.UpdateButtonState( index <= playerDic.Count);
                }
                
                _ = FirestoreManager.Instance.UpdateDataAsync(
                    FirebaseCollections.Rooms, runner.SessionInfo.Properties["RoomId"], userCount);
            }
            else
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

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            if (runner.IsServer)
            {
                // TODO : 연결이 끊어졌습니다 등 표시
                _ = FirestoreManager.Instance.DeleteDataAsync(
                    FirebaseCollections.Rooms, runner.SessionInfo.Properties["RoomId"]);
            }
            
            Debug.Log("호스트가 나가거나 방이 종료됨");
            Destroy(NetworkStartBridge.Instance.gameObject);
            SceneManager.LoadScene(2);
        }
        
        #region INetworkRunnerCallbacks
        
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
        public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
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