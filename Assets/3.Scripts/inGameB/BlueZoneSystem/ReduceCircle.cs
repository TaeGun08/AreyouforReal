using System.Collections;
using UnityEngine;

public class ReduceCircle : MonoBehaviour
{
    [Header("Reduction Settings")]
    public float reductionTime = 45f;
    public float waitTime = 45f;

    private MakeCircle makeCircle;
    public CircleData currentZone;

    private Coroutine reduceCoroutine;

    private void Start()
    {
        makeCircle = GetComponent<MakeCircle>();
        if (makeCircle == null)
        {
            Debug.LogError("MakeCircle 컴포넌트를 찾을 수 없습니다.");
            enabled = false;
            return;
        }

        currentZone = makeCircle.DequeueCircle();
        reduceCoroutine = StartCoroutine(ReduceRoutine());
    }

    private IEnumerator ReduceRoutine()
    {
        yield return new WaitForSeconds(waitTime);

        while (makeCircle.CircleCount > 0)
        {
            var nextZone = makeCircle.DequeueCircle();

            Vector3 startCenter = currentZone.center;
            float startRadius = currentZone.radius;

            Vector3 endCenter = nextZone.center;
            float endRadius = nextZone.radius;

            float elapsed = 0f;
            while (elapsed < reductionTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / reductionTime);

                currentZone.center = Vector3.Lerp(startCenter, endCenter, t);
                currentZone.radius = Mathf.Lerp(startRadius, endRadius, t);

                yield return null;
            }

            if (makeCircle.CircleCount > 0)
                yield return new WaitForSeconds(waitTime);
        }

        Debug.Log("모든 Zone 축소 완료");
        reduceCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        if (currentZone == null) return;

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(currentZone.center, currentZone.radius);
    }
}
