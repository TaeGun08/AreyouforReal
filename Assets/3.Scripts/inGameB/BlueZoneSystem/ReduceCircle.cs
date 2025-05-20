using UnityEngine;

[RequireComponent(typeof(Transform))]
public class ReduceCircle : MonoBehaviour
{
    [Header("MakeCircle")]
    public MakeCircle makeCircle;

    [Header("초")]
    public float shrinkDuration = 90f;

    private bool isShrinking;
    private float startRadius;
    private float targetRadius;
    private float elapsedTime;

    private void Start()
    {
        if (makeCircle == null)
        {
            makeCircle = GetComponent<MakeCircle>();
        }

        BeginShrink(0);
    }

    /// from 단계의 원에서 다음 단계의 원으로 줄어들기 시작합니다.
    public void BeginShrink(int fromIndex)
    {
        if (fromIndex < 0 || fromIndex + 1 >= makeCircle.CircleCount)
        {
            isShrinking = false;
            return;
        }

        startRadius  = makeCircle.GetRadius(fromIndex);
        targetRadius = makeCircle.GetRadius(fromIndex + 1);
        elapsedTime  = 0f;
        isShrinking  = true;

        // 초기 위치와 크기 설정
        transform.position   = makeCircle.GetCenter(fromIndex);
        transform.localScale = Vector3.one * startRadius * 2f;
    }

    private void Update()
    {
        if (!isShrinking) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / shrinkDuration);

        float radius = Mathf.Lerp(startRadius, targetRadius, t);
        transform.localScale = Vector3.one * radius * 2f;

        if (t >= 1f)
        {
            isShrinking = false;
        }
    }

    // Gizmo로 줄어드는 반지름 표시
    private void OnDrawGizmos()
    {
        if (!isShrinking) return;

        // 현재 축소 중인 반지름 (파란색)
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 2f);

        // 목표 반지름 (빨간색)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRadius);
    }
}