using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class AIController : NetworkBehaviour, IKnockout
{  
    private NetworkCharacterController characterController;
    public NetworkCharacterController CharacterController =>  characterController;
    
    private Animator animator;

    [SerializeField] private AIState[] aiState;
    private  AIState currentState;
    
    [SerializeField] private LayerMask groundLayer;
    public bool IsGrounded { get; private set; }

    public NetworkObject NetworkObj => Object;
    
    private Dictionary<AIState.State, AIState> aiStateDic = new Dictionary<AIState.State, AIState>();
    private Dictionary<AIState.State, int> aiAnimDic = new Dictionary<AIState.State, int>();
    
    private void Awake()
    {
        characterController = GetComponent<NetworkCharacterController>();
        animator = GetComponent<Animator>();
        
        for (int i = 0; i < aiState.Length; i++)
        {
            aiStateDic.Add(aiState[i].CurrentState, aiState[i]);
            aiAnimDic.Add(aiState[i].CurrentState, Animator.StringToHash($"{aiState[i].CurrentState}"));
        }
        
        ChangeState(AIState.State.Idle);
        animator.ResetTrigger(aiAnimDic[AIState.State.Idle]);
    }
    
    public override void FixedUpdateNetwork()
    {
        currentState?.StateUpdate();
        GroundChecker();
    }
    
    private void GroundChecker()
    {
        IsGrounded = Physics.Raycast(transform.position
                                   + transform.forward
                                   + Vector3.up, Vector3.down, 10f, groundLayer);
    }
    
    public void ChangeState(AIState.State newState)
    {
        currentState?.StateExit();
        
        currentState = aiStateDic[newState];
        
        currentState.StateEnter(this);
    }
    
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_Knockout()
    {
        characterController.Move(transform.position * (0f * Runner.DeltaTime));
        ChangeState(AIState.State.Knockout);
    }
    
    public void ChangeAnimation(AIState.State newState)
    {
        animator.SetTrigger(aiAnimDic[newState]);
    }

    public void ResetAnimation(AIState.State curState)
    {
        animator.ResetTrigger(aiAnimDic[curState]);
    }
}
