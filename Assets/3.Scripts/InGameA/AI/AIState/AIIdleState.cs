using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class AIIdleState : AIState
{
    public override State CurrentState =>  State.Idle;

    private float timer;

    public override void StateEnter(AIController aiController)
    {
        this.aiController = aiController;
        this.aiController.ChangeAnimation(CurrentState);
        timer = Random.Range(3f, 10f);
    }

    public override void StateUpdate()
    {
        aiController.CharacterController.Move(aiController.transform.position * (0f * Runner.DeltaTime));
        
        timer -= Runner.DeltaTime;
        
        if (timer <= 0f)
        {
            timer = 0f;
            aiController.ChangeState(State.Walk);
        }
    }

    public override void StateExit()
    {
        aiController.ResetAnimation(CurrentState);
    }
}
