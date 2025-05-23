using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using DG.Tweening;

public class AIWalkState : AIState
{
    public override State CurrentState => State.Walk;

    private float Timer { get; set; }
    private Vector3 direction;
    
    public override void StateEnter(AIController aiController)
    {
        this.aiController = aiController;
        this.aiController.ChangeAnimation(CurrentState);
        Timer = Random.Range(3f, 10f);
        direction = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f) * Vector3.forward;
    }

    public override void StateUpdate()
    {
        Vector3 moveTarget = transform.position + direction;
        aiController.Agent.SetDestination(moveTarget);
        
        Timer -= Runner.DeltaTime;
        
        if (Timer <= 0f)
        {
            aiController.ChangeState(State.Idle);
        }
    }

    public override void StateExit()
    {
        aiController.Agent.SetDestination(aiController.transform.position);
        aiController.ResetAnimation(CurrentState);
    }
}
