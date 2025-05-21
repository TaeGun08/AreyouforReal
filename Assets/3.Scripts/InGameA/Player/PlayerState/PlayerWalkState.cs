using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerState
{
    public override State CurrentState => State.Walk;
    
    public override void StateEnter(PlayerController playerController)
    {
        this.playerController = playerController;
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
            
            float speed = playerController.LocalPlayer.Stats.WalkSpeed;
            Vector3 dir = input.Direction.normalized;
            playerController.CharacterController.Move(dir * speed * Runner.DeltaTime);

            if (input.Buttons.IsSet(NetworkInputData.SHIFT_BUTTON_1))
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

    }
}
