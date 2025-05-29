using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class AIManager : NetworkBehaviour
{
    public static AIManager Instance;
    
    [SerializeField] private NetworkPrefabRef AIPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnAI(Vector3 pos)
    {
        Debug.Assert(Runner.IsSharedModeMasterClient, "서버전용");
        
        // TODO : AI 안움직이면 InputAuth 한번 체크해봐야할듯
        Runner.Spawn(AIPrefab, pos, Quaternion.identity);
    }

    public void DespawnAI(NetworkObject networkObject)
    {
        Debug.Assert(Runner.IsSharedModeMasterClient, "서버전용");
        Runner.Despawn(networkObject);
    }
}
