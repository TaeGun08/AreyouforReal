using UnityEngine;
using System.Collections;

public class BlueZoneController : MonoBehaviour
{
    [Header("참조 컴포넌트")]
    public MakeCircle makeCircle;        // 3개 원 생성 모듈
    public ReduceCircle reduceCircle;    // 축소 처리 모듈

    [Header("단계 시간 (초)")]
    public float phaseDuration = 90f;
    private void Start()
    {
        // 자동으로 단계 시퀀스 시작
        StartCoroutine(PhaseSequence());
    }

    private IEnumerator PhaseSequence()
    {
        // 1단계: 아무 동작 없이 대기
        yield return new WaitForSeconds(phaseDuration);

        // 2단계: 첫 번째 원 → 두 번째 원으로 축소
        transform.position = makeCircle.GetCenter(0);
        reduceCircle.BeginShrink(0);
        yield return new WaitForSeconds(phaseDuration);

        // 3단계: 두 번째 원 → 세 번째 원으로 축소
        transform.position = makeCircle.GetCenter(1);
        reduceCircle.BeginShrink(1);
        yield return new WaitForSeconds(phaseDuration);

        // 4단계: (MakeCircle이 3개 원만 생성하므로, 추가 축소 필요 시 아래처럼 호출)
        // transform.position = makeCircle.GetCenter(2);
        // reduceCircle.BeginShrink(2);

        // 전체 페이즈 완료
        Debug.Log("BlueZone 모든 단계 완료");
    }
}