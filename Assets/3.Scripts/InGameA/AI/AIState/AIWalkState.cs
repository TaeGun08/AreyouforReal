using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class AIWalkState : AIState
{
    public override State CurrentState => State.Walk;

    private float timer;
    private Vector3 direction;

    private bool isTurn;

    private int randomState;

    private Coroutine coroutine;

    public override void StateEnter(AIController aiController)
    {
        this.aiController = aiController;
        this.aiController.ChangeAnimation(CurrentState);
        timer = Random.Range(3f, 10f);
        
        randomState = Random.Range(0, 10) % 2;
        
        Vector3 forward = this.aiController.transform.forward;
        float angle = isTurn ? 180f : Random.Range(0f, 360f);
        direction = Quaternion.AngleAxis(angle, Vector3.up) * forward;
        
        isTurn = false;
    }

    public override void StateUpdate()
    {
        aiController.CharacterController.Move(direction.normalized * Runner.DeltaTime);
        
        timer -= Runner.DeltaTime;

        if (aiController.IsGrounded == false)
        {
            isTurn = true;
            switch (randomState)
            {
                case 0:
                    aiController.ChangeState(State.Idle);
                    break;
                case 1:
                    aiController.ChangeState(State.Walk);
                    break;
            }
            return;
        }
        
        if (timer <= 0f)
        {
            timer = 0f;
            switch (randomState)
            {
                case 0:
                    aiController.ChangeState(State.Idle);
                    break;
                case 1:
                    aiController.ChangeState(State.Walk);
                    break;
            }
        }
    }

    public override void StateExit()
    {
        aiController.ResetAnimation(CurrentState);
    }
}
