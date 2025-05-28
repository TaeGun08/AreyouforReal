using System;
using Fusion;
using Photon.Pun;
using UnityEngine;

public class CircleVisualizer : NetworkBehaviour
{
    [Header("자기장 설정")]
    public ReduceCircle reduceCircle;

    [Header("자기장 시각화 프리팹 (반구 등)")]
    public GameObject blueZonePrefab;

    private Transform zoneTransform;
    
    public override void FixedUpdateNetwork()
    {
        if (reduceCircle == null || blueZonePrefab == null) return;

        RPC_ScaleChanger();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ScaleChanger()
    {
        Vector3 center = reduceCircle.currentZone.center;
        float radius = reduceCircle.currentZone.radius;
        
        blueZonePrefab.transform.position = center;
        
        blueZonePrefab.transform.localScale = Vector3.one * radius * 0.35f;
    }
}
