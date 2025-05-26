using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public override State CurrentState => State.Idle;

    public override void StateEnter(PlayerController playerController)
    {
        this.playerController = playerController;
        this.playerController.ChangeAnimation(CurrentState);
    }

    public override void StateUpdate()
    {
        if (playerController.Runner.TryGetInputForPlayer(playerController.Object.InputAuthority,
                out NetworkInputData input))
        {
            if (input.Buttons.IsSet(NetworkInputData.MOUSE_BUTTON_0))
            {
                playerController.ChangeState(State.Attack);
                return;
            }

            Vector3 dir = input.Direction.normalized;
            playerController.CharacterController.Move(dir * 1f * playerController.Runner.DeltaTime);

            if (input.Direction.sqrMagnitude > 0.0f)
                playerController.ChangeState(State.Walk);
        }
    }

    public override void StateExit()
    {
        playerController.ResetAnimation(CurrentState);
    }
}