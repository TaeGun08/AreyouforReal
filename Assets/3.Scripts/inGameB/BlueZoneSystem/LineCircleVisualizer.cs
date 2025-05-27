using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineCircleVisualizer : MonoBehaviour
{
    public ReduceCircle reduceCircle;
    public int segmentCount = 64;
    public float lineWidth = 0.1f;
    
    // "Y축으로 얼마나 띄울지 (라인 시각화 높이 조절)")]
    public float heightOffset = 5f;

    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();

        lr.startWidth = lineWidth;
        lr.endWidth   = lineWidth;
        lr.loop = true;
        lr.positionCount = segmentCount + 1;
    }

    void Update()
    {
        if (reduceCircle == null) return;
        
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
            float y = center.y + heightOffset; // ← Y값에 오프셋 적용
            lr.SetPosition(i, new Vector3(x, y, z));
        }
    }
}