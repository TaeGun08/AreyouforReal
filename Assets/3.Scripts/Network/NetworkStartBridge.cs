using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Firestore;
using Fusion;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;


// 네트워크 관련 
public class NetworkStartBridge : MonoBehaviour
{
    public static NetworkStartBridge Instance { get; private set; }

    [Header("References")]
    [SerializeField] private NetworkRunner runnerPrefab;
    
    private NetworkRunner runner;

    [SerializeField] private int sceneIndex;
    
    private void Awake()
    {
        Instance = this;
    }
    
    public async Task CreateRoom(string roomName, string roomDescription, string maxPlayers)
    {
        runner = Instantiate(runnerPrefab);
        DontDestroyOnLoad(gameObject);
        runner.ProvideInput = true;
        
        runner.AddCallbacks(runner.GetComponent<INetworkRunnerCallbacks>());
        
        SceneRef scene = SceneRef.FromIndex(sceneIndex);
        NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
        
        if (scene.IsValid) 
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }
        
        string roomCode = await Room.CreateRandomCode();
        
        // TODO : 방입장 방생성 분리
        
        var sessionProperty = new Dictionary<string, SessionProperty>()
        {
            { "RoomId", roomCode }
        };
        
        StartGameResult result = await runner.StartGame(new StartGameArgs()
        {
            SessionProperties = sessionProperty,
            GameMode = GameMode.Host,
            SessionName = roomCode,
            Scene = SceneRef.FromIndex(sceneIndex),
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            // playerCount
        });

        if (result.Ok)
        {
            // Debug.Log("방 번호 : " + roomCode);
            
            RoomData roomData = new RoomData()
            {
                RoomName = roomName,
                RoomInfo = roomDescription,
                RoomCode = roomCode,
                CreatedAt = Timestamp.GetCurrentTimestamp(),
                MembersCount = 0, //자기 자신 (Host)
                MaxPlayers = int.Parse(maxPlayers), //숫자만 들어오도록 예외처리하고 있습니다.
                IsGameStarted = false,
                IsGameOver = false,
            };
            
            bool isSucced = await FirestoreManager.Instance.WriteDataAsync<RoomData>(
                FirebaseCollections.Rooms, roomCode, roomData);
            
            if (isSucced == false)
            {
                Debug.Log("파이어베이스 저장실패");
            }
            else
            {
                Debug.Log("생성성공");
            }
        }
        else
        {
            Debug.LogWarning("에러!");
        }
    }
    public async Task JoinRoom(string roomCode)
    {
        runner = Instantiate(runnerPrefab);
        DontDestroyOnLoad(gameObject);
        runner.ProvideInput = true;
        
        runner.AddCallbacks(runner.GetComponent<INetworkRunnerCallbacks>());
        
        SceneRef scene = SceneRef.FromIndex(sceneIndex);
        NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
        
        if (scene.IsValid) 
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        bool canJoin = false;
        
        await FirestoreManager.Instance.ReadDataAsync<RoomData>(FirebaseCollections.Rooms, roomCode).ContinueWithOnMainThread(
            task =>
            {
                RoomData roomData = task.Result;
                
                if (roomData.MembersCount < roomData.MaxPlayers && // 최대인원수 검사
                    roomData.IsGameStarted.Equals(false) &&        // 게임 시작 여부 검사
                    roomData.IsGameOver.Equals(false))             // 게임 종료 여부 검사 //이건 필요한지 후에 생각
                {
                    canJoin =  true;
                }
                else
                {
                    LobbyManager.Instance.OnPopupChecking(CheckTexts.Join);
                }
            });

        if (canJoin)
        {
            StartGameResult result = await runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Client,
                SessionName = roomCode,
                Scene = SceneRef.FromIndex(sceneIndex),
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            }); 
            
            if (result.Ok)
            {
                Dictionary<string, object> updateDic = new Dictionary<string, object> //업데이트할 자료
                {
                    {"MembersCount", FieldValue.Increment(1) } // 1 증가
                };
            
                FirestoreManager.Instance.UpdateDataAsync(FirebaseCollections.Rooms, roomCode ,updateDic);
            }
            else
            {
                Debug.LogWarning("에러!");
            }
        }
    }
}
