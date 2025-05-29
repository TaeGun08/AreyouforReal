using System;
using Fusion;
using UnityEngine;

public class ZoneSystem : NetworkBehaviour
{
    public static ZoneSystem Instance { get; private set; }

    [Header("씬에 배치된 Zone 오브젝트")]
    public GameObject zone;

    [Header("단계별 Zone 스케일 (x=y=z)")]
    public float[] scaleSteps = new float[] { 25f, 15f, 10f, 5f, 2.5f };

    [Tooltip("각 단계 축소에 걸리는 시간 (초)")]
    public float reductionTime = 30f;

    [Tooltip("단계 사이 대기 시간 (초)")]
    public float waitTime = 30f;

    private int currentStepIndex;
    private bool isShrinking;
    private float shrinkStartTime;
    private float nextActionTime;

    public float PlayingTime { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        if (zone == null || scaleSteps.Length == 0) return;

        zone.SetActive(true);
        zone.transform.localScale = Vector3.one * scaleSteps[0];

        currentStepIndex  = 1;
        nextActionTime    = Time.time;
        isShrinking       = false;
    }


    private void Update()
    {
        PlayingTime += Time.deltaTime;
        UpdateZone();
    }

    // public override void FixedUpdateNetwork()
    // {
    //     UpdateZone();
    // }

    private void UpdateZone()
    {
        if (zone == null || scaleSteps.Length == 0)
            return;

        // 마지막 단계 이후엔 waitTime 후 비활성화
        if (currentStepIndex >= scaleSteps.Length)
        {
            if (Time.time >= nextActionTime)
                zone.SetActive(false);
            return;
        }

        // 대기 중인 상태
        if (!isShrinking)
        {
            if (Time.time >= nextActionTime)
            {
                isShrinking      = true;
                shrinkStartTime = Time.time;
            }
        }
        else // 축소 진행 중
        {
            float t           = (Time.time - shrinkStartTime) / reductionTime;
            float startSize   = scaleSteps[currentStepIndex - 1];
            float endSize     = scaleSteps[currentStepIndex];
            float currentSize = Mathf.Lerp(startSize, endSize, t);

            zone.transform.localScale = Vector3.one * currentSize;

            if (t >= 1f)
            {
                zone.transform.localScale = Vector3.one * endSize;
                isShrinking               = false;
                currentStepIndex++;
                nextActionTime            = Time.time + waitTime;
            }
        }
    }
}
