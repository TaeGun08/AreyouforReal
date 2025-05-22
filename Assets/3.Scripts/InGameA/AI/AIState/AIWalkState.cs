using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class AIWalkState : AIState
{
    public override State CurrentState => State.Walk;

    private float Timer { get; set; }

    public override void StateEnter(AIController aiController)
    {
        this.aiController = aiController;
        this.aiController.ChangeAnimation(CurrentState);
        Timer = Random.Range(3f, 10f);
    }

    public override void StateUpdate()
    {
        Vector3 moveTarget = transform.position + transform.forward * 0.2f;
        aiController.Agent.SetDestination(moveTarget);
        
        Timer -= Runner.DeltaTime;
        
        if (Timer <= 0f)
        {
            aiController.ChangeState(State.Idle);
        }
    }

    public override void StateExit()
    {
        aiController.ResetAnimation(CurrentState);
    }
}
