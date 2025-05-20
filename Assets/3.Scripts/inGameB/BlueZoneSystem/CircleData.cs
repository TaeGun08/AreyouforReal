using UnityEngine;

public class CircleData
{
    /// 원의 중심 좌표
    public Vector3 center;

    /// 원의 반지름
    public float radius;

    /// 생성자: 중심과 반지름을 초기화합니다.
    public CircleData(Vector3 center, float radius)
    {
        this.center = center;
        this.radius = radius;
    }
}