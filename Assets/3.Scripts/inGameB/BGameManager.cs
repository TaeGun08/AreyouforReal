using UnityEngine;
using System.Collections;
using Fusion;

public class BGameManager : NetworkBehaviour
{
    public static BGameManager Instance { get; private set; }

    [Header("Zone Components")]
    [SerializeField] private MakeCircle makeCircle;
    [SerializeField] private ReduceCircle reduceCircle;
    public ReduceCircle ReduceCircle => reduceCircle;
    [SerializeField] private CircleVisualizer lineCircleVisualizer;
    private Coroutine survivorCountUpdater;
    
    [Header("Visualizer & UI")]
    [SerializeField] private ZoneUI zoneUI;
    public ZoneUI ZoneUI => zoneUI;
    [SerializeField] private KillLog killLog;

    [SerializeField] private GameObject zone;
    public GameObject Zone => zone;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    /// 게임 시작 시 순서대로 시스템 초기화 및 실행
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_InitializeGame()
    {
        makeCircle.CreateCircles();
        reduceCircle.currentZone = makeCircle.PeekCircle();
        lineCircleVisualizer.reduceCircle = reduceCircle;
        
        zoneUI.Init(reduceCircle.reductionTime, reduceCircle.waitTime);
        
        reduceCircle.StartZoneSystem();
        
        zone.SetActive(true);
        survivorCountUpdater = StartCoroutine(UpdateSurvivorCountRoutine());
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