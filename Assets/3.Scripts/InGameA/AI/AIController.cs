using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class AIController : NetworkBehaviour, IKnockout
{  
    private NavMeshAgent agent;
    public NavMeshAgent Agent => agent;
    
    private Animator animator;

    [SerializeField] private AIState[] aiState;
    private  AIState currentState;
    
    public NetworkObject NetworkObj => Object;
    
    private Dictionary<AIState.State, AIState> aiStateDic = new Dictionary<AIState.State, AIState>();
    private Dictionary<AIState.State, int> aiAnimDic = new Dictionary<AIState.State, int>();
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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
    }
    
    public void ChangeState(AIState.State newState)
    {
        if (currentState != null)
            if (currentState.CurrentState.Equals(newState)) return;
        
        currentState?.StateExit();
        
        currentState = aiStateDic[newState];
        
        currentState.StateEnter(this);
    }
    
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_Knockout()
    {
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
