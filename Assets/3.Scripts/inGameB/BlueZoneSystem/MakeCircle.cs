using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MakeCircle : MonoBehaviour
{
    [Header("Circle Settings")]
    public float initialRadius = 100f;
    public float reduceFactor = 0.75f;
    public float mapRange = 100f;

    private Queue<CircleData> circleQueue = new Queue<CircleData>();

    public int CircleCount => circleQueue.Count;
    public CircleData DequeueCircle() => circleQueue.Dequeue();
    public CircleData PeekCircle() => circleQueue.Peek();

    private void Awake()
    {
        CreateCircles();
    }

    private void CreateCircles()
    {
        CircleData last = null;

        for (int i = 0; i < 4; i++)
        {
            Vector3 center;
            float radius;

            if (i == 0)
            {
                center = GetRandomPosition(mapRange);
                radius = initialRadius;
            }
            else if (i < 3)
            {
                center = GetRandomPositionWithinCircle(last.center, last.radius);
                radius = last.radius * reduceFactor;
            }
            else
            {
                center = GetRandomPositionWithinCircle(last.center, last.radius);
                radius = 15f;
            }

            CircleData data = new CircleData(center, radius);
            circleQueue.Enqueue(data);
            last = data;
        }
    }

    private Vector3 GetRandomPosition(float range)
    {
        float x = Random.Range(-range, range);
        float z = Random.Range(-range, range);
        return new Vector3(x, 0f, z);
    }

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

