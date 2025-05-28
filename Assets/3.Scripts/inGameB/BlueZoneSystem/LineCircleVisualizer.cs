using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class LineCircleVisualizer : MonoBehaviour
{
    [Header("기본 라인 설정 (바닥 원)")]
    public ReduceCircle reduceCircle;
    public int segmentCount = 128;
    public float lineWidth = 0.1f;
    public float heightOffset = 5f;

    [Header("반구 라인 설정")]
    public int verticalSegments = 8;           // 위로 몇 층 쌓을지
    public Material domeLineMaterial;          // 라인용 머티리얼

    [Header("돔 파티클 설정")]
    public ParticleSystem domeParticle;

    private LineRenderer baseLine;
    private List<LineRenderer> domeLines = new List<LineRenderer>();

    void Awake()
    {
        // 바닥 라인 초기화
        baseLine = GetComponent<LineRenderer>();
        baseLine.startWidth = lineWidth;
        baseLine.endWidth = lineWidth;
        baseLine.loop = true;
        baseLine.positionCount = segmentCount + 1;

        // 반구용 라인 오브젝트 생성
        for (int i = 0; i < verticalSegments; i++)
        {
            GameObject go = new GameObject($"DomeLine_{i}");
            go.transform.parent = this.transform;

            LineRenderer lr = go.AddComponent<LineRenderer>();
            lr.material = domeLineMaterial;
            lr.widthMultiplier = lineWidth;
            lr.loop = true;
            lr.positionCount = segmentCount + 1;

            domeLines.Add(lr);
        }
    }

    void Update()
    {
        if (reduceCircle == null) return;

        Vector3 center = reduceCircle.currentZone.center;
        float radius = reduceCircle.currentZone.radius;

        DrawBaseCircle(center, radius);
        DrawDomeLines(center, radius);
        UpdateDomeParticle(center, radius);
    }

    private void DrawBaseCircle(Vector3 center, float radius)
    {
        for (int i = 0; i <= segmentCount; i++)
        {
            float theta = 2f * Mathf.PI * i / segmentCount;
            float x = Mathf.Cos(theta) * radius + center.x;
            float z = Mathf.Sin(theta) * radius + center.z;
            float y = center.y + heightOffset;

            baseLine.SetPosition(i, new Vector3(x, y, z));
        }
    }

    private void DrawDomeLines(Vector3 center, float radius)
    {
        for (int v = 0; v < verticalSegments; v++)
        {
            float t = v / (float)(verticalSegments - 1);
            float angle = Mathf.PI / 2f * t;

            float r = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius + center.y + heightOffset;

            for (int h = 0; h <= segmentCount; h++)
            {
                float theta = 2f * Mathf.PI * h / segmentCount;
                float x = Mathf.Cos(theta) * r + center.x;
                float z = Mathf.Sin(theta) * r + center.z;

                domeLines[v].SetPosition(h, new Vector3(x, y, z));
            }
        }
    }

    private void UpdateDomeParticle(Vector3 center, float radius)
    {
        if (domeParticle == null) return;

        domeParticle.transform.position = center;
        domeParticle.transform.localScale = Vector3.one * radius * 2f;

        if (!domeParticle.isPlaying)
            domeParticle.Play();
    }
}
