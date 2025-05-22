using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class AIIdleState : AIState
{
    public override State CurrentState =>  State.Idle;

    private float Timer { get; set; }

    public override void StateEnter(AIController aiController)
    {
        this.aiController = aiController;
        this.aiController.ChangeAnimation(CurrentState);
        Timer = Random.Range(3f, 10f);
    }

    public override void StateUpdate()
    {
        Timer -= Runner.DeltaTime;
        
        if (Timer <= 0f)
        {
            aiController.ChangeState(State.Walk);
        }
    }

    public override void StateExit()
    {
        aiController.ResetAnimation(CurrentState);
    }
}
