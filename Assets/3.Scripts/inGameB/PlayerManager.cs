using UnityEngine;
using Fusion;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Player Prefabs")]
    public NetworkObject playerPrefab;
    public NetworkObject aiPrefab;

    // 휴먼 플레이어와 AI를 별도로 관리
    public List<NetworkObject> humanPlayers = new List<NetworkObject>();
    public List<NetworkObject> aiPlayers    = new List<NetworkObject>();

    private NetworkRunner runner;

    private const int maxPlayers = 10;
    private const int aiCount    = 20;

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

        if (runner == null)
        {
            Debug.LogError("NetworkRunner를 가져올 수 없습니다.");
            return;
        }

        Debug.Log("NetworkRunner가 성공적으로 초기화되었습니다.");
    }

    // 최대 10명의 휴먼 플레이어 생성
    public void CreatePlayers(int playerCount)
    {
        int createCount = Mathf.Min(playerCount, maxPlayers);

        for (int i = 0; i < createCount; i++)
        {
            var newPlayer = runner.Spawn(playerPrefab, GetRandomPosition(), Quaternion.identity);
            if (newPlayer != null)
            {
                humanPlayers.Add(newPlayer);
                Debug.Log($"Human Player Created: {newPlayer.name} (ID: {newPlayer.Id})");
            }
            else
            {
                Debug.LogError("Player 생성 실패");
            }
        }

        if (playerCount > maxPlayers)
        {
            Debug.LogWarning($"플레이어 수가 최대치를 초과했습니다. 최대 {maxPlayers}명으로 제한합니다.");
        }
    }

    // AI 20명 생성
    public void CreateAIs()
    {
        for (int j = 0; j < aiCount; j++)
        {
            var newAI = runner.Spawn(aiPrefab, GetRandomPosition(), Quaternion.identity);
            if (newAI != null)
            {
                aiPlayers.Add(newAI);
                Debug.Log($"AI Created: {newAI.name} (ID: {newAI.Id})");
            }
            else
            {
                Debug.LogError("AI 생성 실패");
            }
        }
    }

    // 모든 플레이어 및 AI 제거
    public void ClearPlayers()
    {
        foreach (var player in humanPlayers)
        {
            if (player != null)
                runner.Despawn(player);
        }

        foreach (var ai in aiPlayers)
        {
            if (ai != null)
                runner.Despawn(ai);
        }

        humanPlayers.Clear();
        aiPlayers.Clear();

        Debug.Log("모든 휴먼 플레이어 및 AI가 제거되었습니다.");
    }

    private Vector3 GetRandomPosition()
    {
        // y좌표 고정, x/z는 맵 범위에 맞춰 설정하세요
        return new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
    }
}
