using UnityEngine;
using System.Collections;
using Fusion;

public class BGameManager : NetworkBehaviour
{
    public static BGameManager Instance { get; private set; }

    [Header("Zone Components")]
    [SerializeField] private MakeCircle makeCircle;
    [SerializeField] private ReduceCircle reduceCircle;
    [SerializeField] private LineCircleVisualizer lineCircleVisualizer;

    [Header("Visualizer & UI")]
    [SerializeField] private ZoneUI zoneUI;
    [SerializeField] private KillLog killLog;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        InitializeGame();
    }

    /// 게임 시작 시 순서대로 시스템 초기화 및 실행
    public void InitializeGame()
    {
        makeCircle.CreateCircles();
        reduceCircle.currentZone = makeCircle.PeekCircle();
        lineCircleVisualizer.reduceCircle = reduceCircle;
        StartCoroutine(ReduceZoneCoroutine());
    }

    private IEnumerator ReduceZoneCoroutine()
    {
        // ReduceCircle 내부 로직 실행 대기
        yield return StartCoroutine(reduceCircle.ReduceRoutine());

        // 모든 축소 완료 후 처리
        OnGameCleared();
    }

    private void OnGameCleared()
    {
        Debug.Log("모든 자기장 축소 완료 - 게임 클리어 처리");
        // TODO: 승리 화면 전환, 서버 통신 등
    }
}