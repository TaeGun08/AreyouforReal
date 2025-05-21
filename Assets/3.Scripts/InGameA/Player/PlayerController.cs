using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private LocalPlayer localPlayer;
    public LocalPlayer LocalPlayer => localPlayer;

    private NetworkCharacterController characterController;
    public NetworkCharacterController CharacterController => characterController;
    
    private Animator animator;
    
    [SerializeField] private PlayerState[] playerStates;
    private PlayerState currentState;
    
    private Dictionary<PlayerState.State, PlayerState> playerStateDic = new Dictionary<PlayerState.State, PlayerState>();
    private Dictionary<PlayerState.State, int> playerAnimDic = new Dictionary<PlayerState.State, int>();

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
        currentState?.StateUpdate();
    }
    
    public void ChangeState(PlayerState.State newState)
    {
        if (currentState != null)
            if (currentState.CurrentState.Equals(newState)) return;
        
        currentState?.StateExit();
        
        currentState = playerStateDic[newState];
        
        currentState.StateEnter(this);
    }
    
    public void ChangeAnimation(PlayerState.State newState)
    {
        animator.SetTrigger(playerAnimDic[newState]);
    }

    public void ResetAnimation(PlayerState.State curState)
    {
        animator.ResetTrigger(playerAnimDic[curState]);
    }
}
