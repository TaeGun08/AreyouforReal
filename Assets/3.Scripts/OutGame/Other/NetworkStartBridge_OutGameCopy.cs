using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Extensions;
using Fusion;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;
using Random = UnityEngine.Random;


// 네트워크 관련 
public class NetworkStartBridge_OutGameCopy : MonoBehaviour
{
    public static NetworkStartBridge_OutGameCopy Instance { get; private set; }

    [Header("References")]
    [SerializeField] private NetworkRunner runnerPrefab;
    
    private NetworkRunner runner;

    [SerializeField] private int sceneIndex;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public async Task CreateRoom(string roomName, string roomDescription, string maxPlayers)
    {
        if (runner != null)
        {
            await runner.Shutdown();
            Destroy(runner.gameObject);
            runner = null;
        }
        
        runner = Instantiate(runnerPrefab);
        
        runner.ProvideInput = true;
        
        runner.AddCallbacks(runner.GetComponent<INetworkRunnerCallbacks>());
        
        SceneRef scene = SceneRef.FromIndex(sceneIndex);
        NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
        
        if (scene.IsValid) 
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }
        
        string roomCode = Room.CreateRandomCode().Result;
        
        // TODO : 방입장 방생성 분리
        
        string uuid = Guid.NewGuid().ToString();
        
        Debug.Log("생성 UUID : " + uuid);
        
        var sessionProperty = new Dictionary<string, SessionProperty>()
        {
            { "uuid", uuid }
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
            Debug.Log("방 번호 : " + roomCode);
            
            // TODO : 하랑할일
            RoomData roomData = new RoomData()
            {
                RoomName = roomName,
                RoomInfo = roomDescription,
                RoomCode = roomCode,
                MembersCount = 0,
                MaxPlayers = 0,
                IsGameStarted = false,
                IsGameOver = false,
            };
            
            bool isSucced = await FirestoreManager.Instance.WriteDataAsync<RoomData>(
                FirebaseCollections.Rooms, uuid, roomData);
            
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
        
        StartGameResult result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Client,
            SessionName = roomCode,
            Scene = SceneRef.FromIndex(sceneIndex),
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        });

        if (result.Ok)
        {
            
        }
        else
        {
            Debug.LogWarning("에러!");
        }
    }
}
