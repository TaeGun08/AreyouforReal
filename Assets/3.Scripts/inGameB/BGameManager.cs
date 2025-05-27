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
    private Coroutine survivorCountUpdater;
    
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
        survivorCountUpdater = StartCoroutine(UpdateSurvivorCountRoutine());
    }

    /// 게임 시작 시 순서대로 시스템 초기화 및 실행
    public void InitializeGame()
    {
        makeCircle.CreateCircles();
        reduceCircle.currentZone = makeCircle.PeekCircle();
        lineCircleVisualizer.reduceCircle = reduceCircle;
        
        zoneUI.Init(reduceCircle.reductionTime, reduceCircle.waitTime);
        
        reduceCircle.StartZoneSystem();
    }
    private IEnumerator UpdateSurvivorCountRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        while (true)
        {
            int playerCount = reduceCircle.GetCurrentPlayersOnly().Count;
            zoneUI.SetSurvivorCount(playerCount);

            yield return wait;
        }
    }
}