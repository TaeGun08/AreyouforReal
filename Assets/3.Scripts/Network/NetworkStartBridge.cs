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


// 네트워크 관련 
public class NetworkStartBridge : MonoBehaviour
{
    public static NetworkStartBridge Instance { get; private set; }

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
            Debug.Log("방 번호 : " + roomCode);
            
            // TODO : 하랑할일
            
            RoomData roomData = new RoomData()
            {
                RoomName = "eomjunsik",
                RoomCode = roomCode,
                MembersCount = 0,
                IsGameStarted = false,
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
}
