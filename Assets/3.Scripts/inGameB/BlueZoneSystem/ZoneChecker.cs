using System.Collections;
using UnityEngine;
using Fusion;

public class ZoneChecker : NetworkBehaviour
{
    private IZoneTrackable target;
    private ReduceCircle reduceCircle;

    public float checkInterval = 1f;

    public override void Spawned()
    {
        target = GetComponent<IZoneTrackable>();
        reduceCircle = FindObjectOfType<ReduceCircle>();

        bool isPlayer = target is PlayerController;

        // Player는 권한 있는 쪽만 실행, AI는 모두 실행
        if ((!isPlayer || HasStateAuthority) && target != null && reduceCircle != null)
        {
            StartCoroutine(CheckZoneRoutine());
        }
    }

    private IEnumerator CheckZoneRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(checkInterval);

        while (true)
        {
            Vector3 pos = ((MonoBehaviour)target).transform.position;
            Vector3 center = reduceCircle.currentZone.center;
            float radius = reduceCircle.currentZone.radius;

            bool inZone = Vector3.Distance(pos, center) <= radius;
            target.IsInZone = inZone;

            Debug.Log($"[ZoneChecker] {((MonoBehaviour)target).name} → {(inZone ? "IN" : "OUT")}");

            // Knockout 처리 예시
            if (!inZone)
            {
                if (target is PlayerController pc
                    && pc.CurrentState.CurrentState != PlayerState.State.Knockout)
                {
                    pc.ChangeState(PlayerState.State.Knockout);
                }
                else if (target is AIController ai 
                         && ai.CurrentState.CurrentState != AIState.State.Knockout)
                {
                    ai.ChangeState(AIState.State.Knockout);
                }
            }

            yield return wait;
        }
    }
}