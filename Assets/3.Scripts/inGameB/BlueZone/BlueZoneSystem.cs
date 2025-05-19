using UnityEngine;

public class BlueZoneSystem : MonoBehaviour
{
    [Header("Blue Zone Settings")]
    public Vector3 center;
    public float radius;
    public float shrinkSpeed = 0.1f;

    private void Update()
    {
        ShrinkZone();
    }

    // 블루존 축소 메서드
    public void ShrinkZone()
    {
        if (radius > 0)
        {
            radius -= shrinkSpeed * Time.deltaTime;
            radius = Mathf.Max(radius, 0);
            transform.localScale = Vector3.one * radius * 2;
        }
    }

    // 블루존 위치 및 반지름 설정
    public void SetBlueZone(Vector3 newCenter, float newRadius)
    {
        center = newCenter;
        radius = newRadius;
        transform.position = center;
        transform.localScale = Vector3.one * radius * 2;
        Debug.Log($"블루존 이동 - 중심: {center}, 반지름: {radius}");
    }

    // 현재 반지름 반환
    public float GetCurrentRadius()
    {
        return radius;
    }

    // 현재 중심 반환
    public Vector3 GetCurrentCenter()
    {
        return center;
    }
}