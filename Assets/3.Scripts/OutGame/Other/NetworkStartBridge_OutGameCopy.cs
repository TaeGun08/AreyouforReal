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
//룸이 만들어지는 UI

public class NetworkStartBridge_OutGameCopy : MonoBehaviour
{
    public static NetworkStartBridge_OutGameCopy Instance { get; private set; }

    [Header("References")]
    [SerializeField] private NetworkRunner runnerPrefab;
    
    [Space]
    
    [Header("임시용")]
    private string roomCode;
    
    private NetworkRunner runner;

    [SerializeField] private int sceneIndex;
    
    private void Awake()
    {
        Instance = this;
    }

    // CLient시
    public void SetCode(string code)
    {
        roomCode = code;
    }

    // TODO : 매개변수 수정 방이름, 멤버수 
    public async Task StartGame(GameMode mode)
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

        if (roomCode.IsNullOrEmpty())
        {
            roomCode = Room.CreateRandomCode();
        }
        
        string uuid = Guid.NewGuid().ToString();

        Debug.Log("생성 UUID : " + uuid);
        
        var sessionProperty = new Dictionary<string, SessionProperty>()
        {
            { "uuid", uuid }
        };
        
        StartGameResult result = await runner.StartGame(new StartGameArgs()
        {
            SessionProperties = sessionProperty,
            GameMode = mode,
            SessionName = roomCode,
            Scene = SceneRef.FromIndex(sceneIndex),
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            // playerCount
        });

        if (result.Ok)
        {

        }
        else
        {
            Debug.LogWarning("에러!");
        }
    } 
    
    // TODO : 매개변수 수정 방이름, 멤버수 
    public async Task StartGame(GameMode mode, string roomName, string roomInfo, string membersCount)
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

        if (roomCode.IsNullOrEmpty())
        {
            roomCode = Room.CreateRandomCode();
        }
        
        string uuid = Guid.NewGuid().ToString();

        Debug.Log("생성 UUID : " + uuid);
        
        var sessionProperty = new Dictionary<string, SessionProperty>()
        {
            { "uuid", uuid }
        };
        
        StartGameResult result = await runner.StartGame(new StartGameArgs()
        {
            SessionProperties = sessionProperty,
            GameMode = mode,
            SessionName = roomCode,
            Scene = SceneRef.FromIndex(sceneIndex),
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            // playerCount
        });

        if (result.Ok)
        {
            Debug.Log("방 번호 : " + roomCode);
            
            // TODO : 하랑할일
            RoomData roomData = new RoomData() //보낼 데이터 작성
            {
                RoomName = roomName,
                RoomInfo = roomInfo,
                RoomCode = roomCode,
                MembersCount =  int.Parse(membersCount),
                CreatedAt = Timestamp.GetCurrentTimestamp(),
                IsGameStarted = false,
                IsGameOver = false,
            };
            
            //파이어베이스 문서로 작성
            if (await FirestoreManager.Instance.WriteDataAsync<RoomData>(
                    FirebaseCollections.Rooms, uuid, roomData))
            {
                Debug.Log("생성성공");
            }
            else
            {
                Debug.Log("파이어베이스 저장실패");
            }
        }
        else
        {
            Debug.LogWarning("에러!");
        }
    } 
}
