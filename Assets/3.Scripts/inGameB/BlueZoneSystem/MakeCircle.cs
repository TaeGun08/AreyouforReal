using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MakeCircle : MonoBehaviour
{
    [Header("Circle Settings")]
    [Tooltip("초기 반지름")]    public float initialRadius = 100f;
    [Tooltip("이전 원 반지름에 곱할 감소 비율")] public float reduceFactor = 0.75f;
    [Tooltip("첫 번째 원 생성 시 랜덤 위치 범위")] public float mapRange = 100f;

    // 생성된 원 데이터를 저장하는 큐
    private Queue<CircleData> circleQueue = new Queue<CircleData>();

    /// 남아있는 원 개수
    public int CircleCount => circleQueue.Count;

    /// 큐에서 첫 번째 원 정보를 제거하고 반환
    public CircleData DequeueCircle() => circleQueue.Dequeue();

    /// 큐에서 첫 번째 원 정보를 제거하지 않고 반환
    public CircleData PeekCircle() => circleQueue.Peek();

    private void Awake()
    {
        CreateCircles();
    }

    /// 4개의 원을 순차적으로 생성하여 큐에 저장합니다.
    private void CreateCircles()
    {
        CircleData last = null;

        for (int i = 0; i < 4; i++)
        {
            Vector3 center;
            float radius;

            if (i == 0)
            {
                // 첫 번째 원: 전체 맵 범위 내 랜덤 위치, 초기 반지름
                center = GetRandomPosition(mapRange);
                radius = initialRadius;
            }
            else if (i < 3)
            {
                // 두 번째, 세 번째 원: 이전 원 내부 랜덤 위치, 감소된 반지름
                center = GetRandomPositionWithinCircle(last.center, last.radius);
                radius = last.radius * reduceFactor;
            }
            else
            {
                // 네 번째(마지막) 원: 이전 원 내부 랜덤 위치, 고정 반지름
                center = GetRandomPositionWithinCircle(last.center, last.radius);
                radius = 15f;
            }

            // 새 CircleData 생성 및 큐에 추가
            CircleData data = new CircleData(center, radius);
            circleQueue.Enqueue(data);
            last = data;

            Debug.Log($"원 {i + 1} - 중심: {center}, 반지름: {radius}");
        }
    }

    /// 맵 범위 내 랜덤 위치 생성
    private Vector3 GetRandomPosition(float range)
    {
        float x = Random.Range(-range, range);
        float z = Random.Range(-range, range);
        return new Vector3(x, 0f, z);
    }

    /// 이전 원 내부의 랜덤 위치 생성
    private Vector3 GetRandomPositionWithinCircle(Vector3 origin, float radius, float maxDistanceFactor = 0.25f)
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * radius * maxDistanceFactor;
        float x = origin.x + Mathf.Cos(angle) * distance;
        float z = origin.z + Mathf.Sin(angle) * distance;
        return new Vector3(x, 0f, z);
    }

 private void OnDrawGizmos()
 {
     if (circleQueue == null || circleQueue.Count == 0)
         return;

     Color[] colors = { Color.blue, Color.green, Color.red, Color.black };
     int i = 0;

     foreach (var circle in circleQueue)
     {
         Gizmos.color = colors[i % colors.Length];
         Gizmos.DrawWireSphere(circle.center, circle.radius);
         i++;
     }
 }
}
