using UnityEngine;
using System.Collections;

public class PhaseSystem : MonoBehaviour
{
    [Header("Phase Settings")]
    public Vector3 initialCenter = Vector3.zero;
    public float initialRadius = 50f;
    public float[] shrinkSpeeds = { 0f, 0.1f, 0.3f, 0.5f };
    public int phaseDuration = 90;
    public float minFinalRadius = 5f;
    public float fixedHeight = 100f; // Y값 고정 높이

    private int currentPhase = 0;
    private float currentRadius;
    private float phaseTimer;
    private Vector3 center;

    private void Start()
    {
        center = initialCenter;
        currentRadius = initialRadius;
        phaseTimer = phaseDuration;
        transform.position = center;
        SetSphereScale(currentRadius); // 초기 크기 설정
        StartCoroutine(PhaseControl());
    }

    private IEnumerator PhaseControl()
    {
        while (currentPhase < 4)
        {
            StartPhase(currentPhase);
            yield return new WaitForSeconds(phaseDuration);
            NextPhase();
        }
        Debug.Log("모든 블루존 페이즈가 완료되었습니다.");
    }

    private void StartPhase(int phase)
    {
        Debug.Log($"[Phase {phase + 1}] 시작");

        if (phase == 0)
        {
            SetBlueZone(initialCenter, initialRadius);
        }
        else
        {
            float newRadius = Mathf.Max(currentRadius * 0.8f, minFinalRadius);
            Vector3 newCenter = GetConstrainedPosition(center, currentRadius, newRadius);
            SetBlueZone(newCenter, newRadius);
        }
    }

    private void SetBlueZone(Vector3 newCenter, float newRadius)
    {
        center = newCenter;
        currentRadius = newRadius;
        transform.position = center;
        SetSphereScale(currentRadius);
        Debug.Log($"블루존 설정 - 중심: {center}, 반지름: {currentRadius}");
    }

    private void NextPhase()
    {
        currentPhase++;
        if (currentPhase >= shrinkSpeeds.Length)
        {
            Debug.Log("최종 단계 도달 - 축소 중지");
            currentPhase = shrinkSpeeds.Length - 1;
            return;
        }
        phaseTimer = phaseDuration;
        StartPhase(currentPhase);
    }

    private void Update()
    {
        if (currentPhase == 0 || currentPhase >= shrinkSpeeds.Length) return;

        if (currentRadius > minFinalRadius)
        {
            currentRadius -= shrinkSpeeds[currentPhase] * Time.deltaTime;
            currentRadius = Mathf.Max(currentRadius, minFinalRadius);
            SetSphereScale(currentRadius);
        }
    }

    // X, Z 값만 줄어들고 Y는 고정된 상태로 설정
    private void SetSphereScale(float radius)
    {
        transform.localScale = new Vector3(radius * 2, fixedHeight, radius * 2);
    }

    private Vector3 GetConstrainedPosition(Vector3 origin, float currentRadius, float nextRadius)
    {
        float maxMoveDistance = currentRadius - nextRadius;
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0;
        float distance = Random.Range(0, maxMoveDistance);
        Vector3 newPosition = origin + randomDirection.normalized * distance;
        return newPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(center, currentRadius);
    }
}
