using System.Collections;
using System.Collections.Generic;
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
#if UNITY_EDITOR || UNITY_STANDALONE
        if (GetInput(out NetworkInputData input))
        {
            if (input.Buttons.IsSet(NetworkInputData.MOUSE_BUTTON_0))
            {
                playerController.ChangeState(State.Attack);
                return;
            }
            
            float angles = Mathf.Atan2(input.Direction.x, input.Direction.z) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
            Vector3 dir = Quaternion.Euler(0f, angles, 0f) * Vector3.forward;
            playerController.CharacterController.Move(dir * Runner.DeltaTime);
            
            if (input.IsRun)
            {
                playerController.ChangeState(State.Run);
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