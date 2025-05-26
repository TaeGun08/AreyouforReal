using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerWalkState : PlayerState
{
    public override State CurrentState => State.Walk;

    public override void StateEnter(PlayerController playerController)
    {
        this.playerController = playerController;
        this.playerController.ChangeAnimation(CurrentState);
        this.playerController.CharacterController.maxSpeed = this.playerController.LocalPlayer.Stats.WalkSpeed;
    }

    public override void StateUpdate()
    {
        if (playerController.Runner.TryGetInputForPlayer(playerController.Object.InputAuthority, out NetworkInputData input))
        {
            if (input.Buttons.IsSet(NetworkInputData.MOUSE_BUTTON_0))
            {
                playerController.ChangeState(State.Attack);
                return;
            }
            
            playerController.CharacterController.Move(input.Direction * playerController.Runner.DeltaTime);
            
            if (input.IsRun)
            {
                playerController.ChangeState(State.Run);
                return;
            }

            if (input.Direction.sqrMagnitude <= 0.0f)
                playerController.ChangeState(State.Idle);
        }
    }

    public override void StateExit()
    {
        playerController.ResetAnimation(CurrentState);
    }
}