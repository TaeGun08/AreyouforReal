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
    }

    public override void StateUpdate()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (GetInput(out NetworkInputData input))
        {
            if (input.Buttons.IsSet(NetworkInputData.MOUSE_BUTTON_0))
            {
                playerController.ChangeState(State.Attack);
                return;
            }

            float speed = playerController.LocalPlayer.Stats.RunSpeed;
            Vector3 dir = input.Direction.normalized;
            playerController.CharacterController.Move(dir * speed * Runner.DeltaTime);
            
            if (input.IsRun == false)
            {
                playerController.ChangeState(State.Walk);
                return;
            }

            if (input.Direction.sqrMagnitude <= 0.0f)
                playerController.ChangeState(State.Idle);
        }
#else
#endif
    }

    public override void StateExit()
    {
        playerController.ResetAnimation(CurrentState);
    }
}