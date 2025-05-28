using UnityEngine;

public class CircleVisualizer : MonoBehaviour
{
    [Header("자기장 설정")]
    public ReduceCircle reduceCircle;

    [Header("자기장 시각화 프리팹 (반구 등)")]
    public GameObject blueZonePrefab;

    private Transform zoneTransform;

    void Start()
    {
        if (blueZonePrefab != null)
        {
            // 프리팹 인스턴스 생성 후 자식으로 둠
            GameObject zoneInstance = Instantiate(blueZonePrefab, transform);
            zoneTransform = zoneInstance.transform;
        }
    }

    void Update()
    {
        if (reduceCircle == null || zoneTransform == null) return;

        Vector3 center = reduceCircle.currentZone.center;
        float radius = reduceCircle.currentZone.radius;

        // 위치 동기화
        zoneTransform.position = center;

        // 크기 동기화 (반지름 기준이므로 * 2)
        zoneTransform.localScale = Vector3.one * radius * 2f;
    }
}
