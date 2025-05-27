using System;
using System.Collections;
using UnityEngine;
using Fusion;

public class ZoneChecker : NetworkBehaviour
{
    private IZoneTrackable target;
    private ReduceCircle reduceCircle;

    //public float checkInterval = 1f;

    private void Start()
    {
        reduceCircle = BGameManager.Instance.ReduceCircle;
        
        target = GetComponent<IZoneTrackable>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GameManager_Network.Instance.State != GameManager_Network.GameState.Play
            && BGameManager.Instance.Zone.activeInHierarchy == false) return;
        if (reduceCircle == null) return;

        Vector3 pos = ((MonoBehaviour)target).transform.position;
        Vector3 center = reduceCircle.currentZone.center;
        float radius = reduceCircle.currentZone.radius;

        bool inZone = Vector3.Distance(pos, center) <= radius;
        target.IsInZone = inZone;

        //Debug.Log($"[ZoneChecker] {((MonoBehaviour) target).name} → {(inZone ? "IN" : "OUT")}");

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
    }

    // private IEnumerator CheckZoneRoutine()
    // {
    //     WaitForSeconds wait = new WaitForSeconds(checkInterval);
    //
    //     while (true)
    //     {
    //         Vector3 pos = ((MonoBehaviour)target).transform.position;
    //         Vector3 center = reduceCircle.currentZone.center;
    //         float radius = reduceCircle.currentZone.radius;
    //
    //         bool inZone = Vector3.Distance(pos, center) <= radius;
    //         target.IsInZone = inZone;
    //
    //         Debug.Log($"[ZoneChecker] {((MonoBehaviour)target).name} → {(inZone ? "IN" : "OUT")}");
    //
    //         // Knockout 처리 예시
    //         if (!inZone)
    //         {
    //             if (target is PlayerController pc
    //                 && pc.CurrentState.CurrentState != PlayerState.State.Knockout)
    //             {
    //                 pc.ChangeState(PlayerState.State.Knockout);
    //             }
    //             else if (target is AIController ai 
    //                      && ai.CurrentState.CurrentState != AIState.State.Knockout)
    //             {
    //                 ai.ChangeState(AIState.State.Knockout);
    //             }
    //         }
    //
    //         yield return wait;
    //     }
    // }
}