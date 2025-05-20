using System;
using System.Linq;
using System.Threading.Tasks;
using Fusion;
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

    public async Task StartGame(GameMode mode)
    {
        runner = Instantiate(runnerPrefab);
        DontDestroyOnLoad(gameObject);
        runner.ProvideInput = true;
        
        runner.AddCallbacks(runner.GetComponent<INetworkRunnerCallbacks>());
        
        SceneRef scene = SceneRef.FromPath("TestScene_Network_InGame");
        NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
        
        if (scene.IsValid) 
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        if (roomCode.IsNullOrEmpty())
        {
            roomCode = Room.CreateRandomCode();
        }
        
        StartGameResult result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomCode,
            Scene = SceneRef.FromIndex(sceneIndex),
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        });

        if (result.Ok)
        {
            Debug.Log("방 번호 : " + roomCode);
        }
        else
        {
            Debug.LogWarning("에러!");
        }
    } 
}
