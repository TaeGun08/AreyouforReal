using System.Collections;
using UnityEngine;

public class ReduceCircle : MonoBehaviour
{
    [Header("Reduction Settings")]
    [Tooltip("한 Zone이 줄어드는 데 걸리는 시간 (초)")]
    public float reductionTime = 10f;

    [Tooltip("다음 Zone으로 넘어가기 전 대기 시간 (초)")]
    public float waitTime = 10f;

    private MakeCircle makeCircle;
    private CircleData currentZone;

    private void Start()
    {
        makeCircle = GetComponent<MakeCircle>();
        if (makeCircle == null)
        {
            Debug.LogError("MakeCircle 컴포넌트를 찾을 수 없습니다.");
            enabled = false;
            return;
        }

        // 첫 Zone 꺼내서 currentZone으로 설정
        currentZone = makeCircle.DequeueCircle();
        StartCoroutine(ReduceRoutine());
    }

    private IEnumerator ReduceRoutine()
    {
        // 첫 Zone 대기
        yield return new WaitForSeconds(waitTime);

        // 남은 Zone이 있을 때까지 반복
        while (makeCircle.CircleCount > 0)
        {
            // 다음 Zone 정보(대상) 꺼내기
            var nextZone = makeCircle.DequeueCircle();

            // 애니메이션 시작 전 초기값 저장
            Vector3 startCenter = currentZone.center;
            float   startRadius = currentZone.radius;

            Vector3 endCenter = nextZone.center;
            float   endRadius = nextZone.radius;

            float elapsed = 0f;
            while (elapsed < reductionTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / reductionTime);

                // 중심과 반지름을 부드럽게 보간
                currentZone.center = Vector3.Lerp(startCenter, endCenter, t);
                currentZone.radius = Mathf.Lerp(startRadius, endRadius, t);

                yield return null;
            }

            // 다음 Zone 시작 전 대기
            if (makeCircle.CircleCount > 0)
                yield return new WaitForSeconds(waitTime);
        }

        Debug.Log("모든 Zone 축소 완료");
    }

    private void OnDrawGizmos()
    {
        if (currentZone == null)
            return;

        // 런타임 중 하나의 Zone만 시각화
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(currentZone.center, currentZone.radius);
    }
}
