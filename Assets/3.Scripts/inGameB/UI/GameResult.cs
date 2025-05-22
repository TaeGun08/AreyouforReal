using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// 게임 종료 시 순위 산정 및 기록을 담당하는 싱글톤 매니저
public class GameResult : MonoBehaviour
{
    public static GameResult Instance { get; private set; }

    // 퇴장된 순서대로 담기는 큐
    private readonly Queue<PlayerController> eliminationQueue = new Queue<PlayerController>();
    private int totalPlayers;

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

    /// 게임 시작 시 전체 참가자 수를 초기화
    public void Initialize(int totalPlayers)
    {
        this.totalPlayers = totalPlayers;
        eliminationQueue.Clear();
    }

    /// KnockoutState.Enter() 호출로 플레이어 퇴장을 기록
    public void RecordElimination(PlayerController player)
    {
        eliminationQueue.Enqueue(player);
    }

    /// 최종 순위를 {PlayerController → 순위} 형태로 반환
    public Dictionary<PlayerController, int> GetRankings()
    {
        var rankings = new Dictionary<PlayerController, int>();
        int rank = totalPlayers;  // 첫 번째 퇴장은 최하위

        foreach (var p in eliminationQueue)
        {
            rankings[p] = rank--;
        }

        return rankings;
    }
}