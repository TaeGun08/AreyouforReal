using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineCircleVisualizer : MonoBehaviour
{
    [Tooltip("줄어드는 Zone 정보를 가진 ReduceCircle")]
    public ReduceCircle reduceCircle;

    [Tooltip("원 둘레를 몇 개의 선분으로 나눌지")]
    public int segmentCount = 64;

    [Tooltip("라인 두께")]
    public float lineWidth = 0.1f;

    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();

        // 라인 두께 설정
        lr.startWidth = lineWidth;
        lr.endWidth   = lineWidth;

        // 선을 loop(폐곡선)으로
        lr.loop = true;

        // 포인트 개수를 segmentCount+1 로 설정
        lr.positionCount = segmentCount + 1;
    }

    void Update()
    {
        if (reduceCircle == null) return;

        // 현재 Zone 정보를 가져와서 원 그리기
        Vector3 center = reduceCircle.currentZone.center;
        float   radius = reduceCircle.currentZone.radius;
        DrawCircle(center, radius);
    }

    private void DrawCircle(Vector3 center, float radius)
    {
        for (int i = 0; i <= segmentCount; i++)
        {
            float theta = 2f * Mathf.PI * i / segmentCount;
            float x = Mathf.Cos(theta) * radius + center.x;
            float z = Mathf.Sin(theta) * radius + center.z;
            lr.SetPosition(i, new Vector3(x, center.y, z));
        }
    }
}