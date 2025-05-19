using UnityEngine;
using Fusion;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Player Prefabs")]
    public NetworkObject playerPrefab;
    public NetworkObject aiPrefab;

    public List<NetworkObject> players = new List<NetworkObject>();
    private NetworkRunner runner;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        ResetRunner();
    }
    private void ResetRunner()
    {
        runner = GetComponent<NetworkRunner>();

        if (runner == null)
        {
            runner = gameObject.AddComponent<NetworkRunner>();
            Debug.Log("NetworkRunner가 추가되었습니다.");
        }
        // Runner가 올바르게 초기화되지 않았을 경우
        if (runner == null)
        {
            Debug.LogError("NetworkRunner를 가져올 수 없습니다.");
            return;
        }
        Debug.Log("NetworkRunner가 성공적으로 초기화되었습니다.");
    }

    // 플레이어와 AI 생성
    public void CreatePlayers(int playerCount, int aiCount)
    {
        for (int i = 0; i < playerCount; i++)
        {
            var newPlayer = runner.Spawn(playerPrefab, GetRandomPosition(), Quaternion.identity);
            if (newPlayer != null)
            {
                players.Add(newPlayer);
                Debug.Log("Player Created: " + newPlayer.name);
            }
            else
            {
                Debug.LogError("Player 생성 실패");
            }
        }

        for (int j = 0; j < aiCount; j++)
        {
            var newAI = runner.Spawn(aiPrefab, GetRandomPosition(), Quaternion.identity);
            if (newAI != null)
            {
                players.Add(newAI);
                Debug.Log("AI Created: " + newAI.name);
            }
            else
            {
                Debug.LogError("AI 생성 실패");
            }
        }
    }
    public void ClearPlayers()
    {
        foreach (var player in players)
        {
            if (player != null)
            {
                runner.Despawn(player);
                Debug.Log("Player/AI Removed: " + player.name);
            }
        }
        players.Clear();
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
    }
}
