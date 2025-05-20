using UnityEngine;

public class MakeCircle : MonoBehaviour
{
    [Header("Circle Settings")]
    public float initialRadius = 100f;
    public float reduceFactor = 0.75f; // 반지름 감소 비율
    public float mapRange = 100f;     // 첫 번째 원 랜덤 생성 범위

    private Vector3[] centers = new Vector3[3];
    private float[] radii = new float[3];
    
    public int CircleCount => radii.Length;
    public Vector3 GetCenter(int idx) => centers[idx];
    public float   GetRadius(int idx) => radii[idx];
    private void Start()
    {
        // 3개의 원 생성
        CreateCircles();
    }

    // 원 생성 메서드
    private void CreateCircles()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == 0)
            {
                // 첫 번째 원: 맵 전체 범위에서 랜덤 중심 설정
                centers[i] = GetRandomPosition(mapRange);
                radii[i] = initialRadius;
            }
            else
            {
                // 두 번째, 세 번째 원: 이전 원 내부에서 랜덤 중심 설정
                float newRadius = radii[i - 1] * reduceFactor;
                centers[i] = GetRandomPositionWithinCircle(centers[i - 1], radii[i - 1]);
                radii[i] = newRadius;
            }

            Debug.Log($"원 {i + 1} - 중심: {centers[i]}, 반지름: {radii[i]}");
        }
    }

    // 첫 번째 원 랜덤 위치 생성 (맵 범위 내)
    private Vector3 GetRandomPosition(float range)
    {
        float x = Random.Range(-range, range);
        float z = Random.Range(-range, range);
        return new Vector3(x, 0, z);
    }

    // 이전 원 내부에서 새로운 중심 설정
    private Vector3 GetRandomPositionWithinCircle(Vector3 origin, float radius, float maxDistanceFactor = 0.25f)
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        // 중심 근처 확률을 높이기 위한 거리 계산
        float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * radius * maxDistanceFactor;
        float x = origin.x + Mathf.Cos(angle) * distance;
        float z = origin.z + Mathf.Sin(angle) * distance;
        return new Vector3(x, 0, z);
    }


    // Gizmo로 원 시각화
    // private void OnDrawGizmos()
    // {
    //     if (centers == null || radii == null) return;
    
    //     Color[] colors = { Color.blue, Color.green, Color.red };
    
    //     for (int i = 0; i < 3; i++)
    //     {
    //         Gizmos.color = colors[i];
    //         Gizmos.DrawWireSphere(centers[i], radii[i]);
    //     }
    // }
}
