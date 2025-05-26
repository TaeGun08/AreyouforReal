using Fusion;
using UnityEngine;

public class ZoneManager : NetworkBehaviour
{
    public static ZoneManager Instance { get; private set; }

    [Header("자기장 구성 요소")]
    [SerializeField] private MakeCircle makeCircle; 
    [SerializeField] private ReduceCircle reduceCircle;
    [SerializeField] private LineCircleVisualizer lineVisualizer;

    [Header("UI")]
    [SerializeField] private ZoneUI zoneUI;

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

  //   public void StartZone()
  //   {
  //       reduceCircle?.StartReduce();
  //       if (lineVisualizer != null) lineVisualizer.enabled = true;
  //       if (zoneUI != null) zoneUI.enabled = true; // UI 업데이트 시작
  //   }
// 
  //   public void StopZone()
  //   {
  //       reduceCircle?.StopReduce();
  //       if (lineVisualizer != null) lineVisualizer.enabled = false;
  //       if (zoneUI != null) zoneUI.enabled = false; // UI 업데이트 중단
  //   }
}
