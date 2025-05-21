using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public override State CurrentState => State.Idle;

    public override void StateEnter(PlayerController playerController)
    {
        this.playerController = playerController;
        this.playerController.CurrentState = CurrentState;
    }

    public override void StateUpdate()
    {
        if (GetInput(out NetworkInputData input))
        {
            if (input.IsAttack)
            {
                playerController.ChangeState(State.Attack);
                return;
            }
            
            Vector3 dir = new Vector3(input.Horizontal, 0, input.Vertical).normalized;
            playerController.CharacterController.Move(dir * 1f * Runner.DeltaTime);

            if (input.IsRun)
            {
                playerController.ChangeState(State.Run);
                return;
            }
            
            if (input.Horizontal != 0 || input.Vertical != 0)
                playerController.ChangeState(State.Walk);
        }
    }

    public override void StateExit()
    {
    }
}