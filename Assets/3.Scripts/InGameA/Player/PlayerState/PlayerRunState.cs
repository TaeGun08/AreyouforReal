using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    public override State CurrentState => State.Run;

    public override void StateEnter(PlayerController playerController)
    {
        this.playerController = playerController;
        this.playerController.ChangeAnimation(CurrentState);
        this.playerController.CharacterController.maxSpeed = this.playerController.LocalPlayer.Stats.RunSpeed;
    }

    public override void StateUpdate()
    {
        if (playerController.Runner.TryGetInputForPlayer(playerController.Object.InputAuthority, out NetworkInputData input))
        {
            if (input.Buttons.IsSet(NetworkInputData.MOUSE_BUTTON_0) 
                && GameManager_Network.Instance.State == GameManager_Network.GameState.Play)
            {
                playerController.ChangeState(State.Attack);
                return;
            }
            
            playerController.CharacterController.Move(input.Direction * playerController.Runner.DeltaTime);
            
            if (input.IsRun == false)
            {
                playerController.ChangeState(State.Walk);
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