using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceCircle : MonoBehaviour
{
    [Header("Reduction Settings")]
    public float reductionTime = 45f;
    public float waitTime = 45f;

    [Header("Detection Settings")]
    public LayerMask detectionLayer;         // 감지할 레이어 (Player, AI)
    public float checkInterval = 1f;         // 생존자 감지 간격 (초)

    [SerializeField] private MakeCircle makeCircle;
    public CircleData currentZone;

    private Coroutine reduceCoroutine;
    private Coroutine detectionCoroutine;

    private readonly List<GameObject> currentSurvivors = new List<GameObject>();
    
    private void Awake()
    {
        if (makeCircle == null)
            makeCircle = FindObjectOfType<MakeCircle>();
    }

    private void Start()
    {
        if (makeCircle == null)
        {
            Debug.LogError("MakeCircle 컴포넌트를 찾을 수 없습니다.");
            enabled = false;
            return;
        }
        
        currentZone = makeCircle.DequeueCircle();
        reduceCoroutine = StartCoroutine(ReduceRoutine());
    }

    public IEnumerator ReduceRoutine()
    {
        yield return new WaitForSeconds(waitTime);

        while (makeCircle.CircleCount > 0)
        {
            var nextZone = makeCircle.DequeueCircle();

            Vector3 startCenter = currentZone.center;
            float startRadius = currentZone.radius;

            Vector3 endCenter = nextZone.center;
            float endRadius = nextZone.radius;

            // 감지 코루틴 시작
            if (detectionCoroutine != null)
                StopCoroutine(detectionCoroutine);
            detectionCoroutine = StartCoroutine(DetectSurvivorsRoutine());

            float elapsed = 0f;
            while (elapsed < reductionTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / reductionTime);

                currentZone.center = Vector3.Lerp(startCenter, endCenter, t);
                currentZone.radius = Mathf.Lerp(startRadius, endRadius, t);

                yield return null;
            }

            if (makeCircle.CircleCount > 0)
                yield return new WaitForSeconds(waitTime);
        }

        if (detectionCoroutine != null)
            StopCoroutine(detectionCoroutine);

        Debug.Log("모든 Zone 축소 완료");
        reduceCoroutine = null;
    }


    private IEnumerator DetectSurvivorsRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(checkInterval);

        while (true)
        {
            currentSurvivors.Clear();

            foreach (var col in Physics.OverlapSphere(currentZone.center, currentZone.radius, detectionLayer))
            {
                Debug.Log($"[감지됨] {col.name}, Layer: {LayerMask.LayerToName(col.gameObject.layer)}");

                currentSurvivors.Add(col.gameObject);
            }

            Debug.Log($"생존자 수 (Player만 필터링 시): {GetCurrentPlayersOnly().Count}");

            yield return wait;
        }
    }
    public List<GameObject> GetCurrentPlayersOnly()
    {
        return currentSurvivors.FindAll(go => go.CompareTag("Player"));
    }
    public void StartZoneSystem()
    {
        currentZone = makeCircle.DequeueCircle();
        StartCoroutine(ReduceRoutine());
        StartCoroutine(DetectSurvivorsRoutine());
    }
    

    private void OnDrawGizmos()
    {
        if (currentZone == null) return;
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(currentZone.center, currentZone.radius);
    }
}
