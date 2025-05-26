using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Fusion;
using Unity.Services.Authentication;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager_Network : NetworkBehaviour
{
    private const int MAX_PLAYER = 10;
    public static GameManager_Network Instance { get; private set; }
    [Networked] TickTimer Delay { get; set; }
    
    [Networked, Capacity(MAX_PLAYER)]
    public NetworkLinkedList<PlayerRef> AlivePlayers { get; }
    
    [Networked]
    [field: SerializeField]
    private int aliveAICount { get; set; } 

    [SerializeField] private GameObject map;
    [Networked] private GameState delayedState { get; set; }
    public enum GameState
    {
        None,
        Wait,
        Start,
        Play,
        End,
    }
    
    public GameState State => state;
    
    [Networked]
    private GameState state { get; set; }

    private void Awake()
    {
        Instance = this;
        state = GameState.Wait;
    }

    public override void FixedUpdateNetwork()
    {
        if (GameState.None < delayedState && Delay.ExpiredOrNotRunning(Runner))
        {
            state = delayedState;
            delayedState = GameState.None;
        }
    }

    public bool TryStartGame()
    {
        Debug.Assert(Runner.IsServer, "서버만 호출가능!");
        
        if (2 <= PlayerRegistry.Instance.playerDic.Count)
        {
            // TODO : 로딩화면 활성화, AI 스폰, 자기장
            // AIManager.Instance.SpawnAI(
            //     TelpoTransform.Instance.TelepoTrs[Random.Range(0, TelpoTransform.Instance.TelepoTrs.Length)].position);
            RPC_MapActive();
            state = GameState.Start;
            
            foreach (var player in PlayerRegistry.Instance.playerDic)
            {
                Transform trs = TelpoTransform.Instance.TelepoTrs[Random.Range(0, TelpoTransform.Instance.TelepoTrs.Length)];
                
                // 캐싱필요
                player.Value.GetComponent<NetworkCharacterController>().Teleport(trs.position);
                AlivePlayers.Add(player.Key);
            }
            
            DelaySetState(GameState.Play, 5);
            
            return true;
        }
        
        return false;
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_MapActive()
    {
        map.SetActive(true);
    }
    
    private void DelaySetState(GameState state, float delayTime)
    {
        Delay = TickTimer.CreateFromSeconds(Runner, delayTime);
        delayedState = state;
    }

    public void KillEvent(PlayerRef player)
    {
        if (AlivePlayers.Remove(player))
        {
            if (AlivePlayers.Count <= 1)
            {
                EndGame();
            }
        }
    }

    private void EndGame()
    {
        state = GameState.End;
        
        // TODO : 전적표시
        
        foreach (var player in PlayerRegistry.Instance.playerDic)
        {
            // 라운지로 이동
            player.Value.GetComponent<NetworkCharacterController>().Teleport(new Vector3(0, 2, 0));
            AlivePlayers.Add(player.Key);
        }
        
    }
}
