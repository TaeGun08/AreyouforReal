using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour, IKnockout, IZoneTrackable
{
    private LocalPlayer localPlayer;
    public LocalPlayer LocalPlayer => localPlayer;

    private NetworkCharacterController characterController;
    public NetworkCharacterController CharacterController => characterController;

    public NetworkObject NetworkObj => Object;

    private Animator animator;

    [SerializeField] private PlayerState[] playerStates;
    private PlayerState currentState;
    public PlayerState CurrentState => currentState;

    private Dictionary<PlayerState.State, PlayerState>
        playerStateDic = new Dictionary<PlayerState.State, PlayerState>();

    private Dictionary<PlayerState.State, int> playerAnimDic = new Dictionary<PlayerState.State, int>();
    
    public int KillCount { get; set; }

    // 킬로그용
    public static event Action<PlayerController, PlayerController> OnPlayerKnockoutEvent;
    public bool IsInZone { get; set; }
    private void Awake()
    {
        localPlayer = GetComponent<LocalPlayer>();
        characterController = GetComponent<NetworkCharacterController>();
        animator = GetComponent<Animator>();

        for (int i = 0; i < playerStates.Length; i++)
        {
            playerStateDic.Add(playerStates[i].CurrentState, playerStates[i]);
            playerAnimDic.Add(playerStates[i].CurrentState, Animator.StringToHash($"{playerStates[i].CurrentState}"));
        }

        ChangeState(PlayerState.State.Idle);
        animator.ResetTrigger(playerAnimDic[PlayerState.State.Idle]);
    }

    public override void FixedUpdateNetwork()
    {
        if (GameManager_Network.Instance.State == GameManager_Network.GameState.Start) return;
        currentState?.StateUpdate();
    }

    public void ChangeState(PlayerState.State newState)
    {
        if (currentState != null)
            if (currentState.CurrentState.Equals(newState))
                return;

        currentState?.StateExit();

        currentState = playerStateDic[newState];

        currentState.StateEnter(this);
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_Knockout()
    {
        ChangeState(PlayerState.State.Knockout);
    }

    public void ChangeAnimation(PlayerState.State newState)
    {
        animator.SetTrigger(playerAnimDic[newState]);
    }

    public void ResetAnimation(PlayerState.State curState)
    {
        animator.ResetTrigger(playerAnimDic[curState]);
    }

    // 킬로그용 영돈
    // public void RPC_PlayerKnockout(NetworkObjectRef attackerRef)
    // {
    //     ChangeState(PlayerState.State.Knockout);
    //     ChangeAnimation(PlayerState.State.Knockout);
//
    //     var attackerObj = Runner.GetPlayerObject(attackerRef)?
    //         .GetComponent<PlayerController>();
    //     if (attackerObj != null)
    //         OnPlayerKnockoutEvent?.Invoke(attackerObj, this);
    // }
}