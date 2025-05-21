using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerState
{
    public override State CurrentState => State.Walk;
    
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
                playerController.ChangeState(State.Run);
                return;
            }
            
            float speed = playerController.LocalPlayer.Stats.WalkSpeed;
            Vector3 dir = new Vector3(input.Horizontal, 0, input.Vertical).normalized;
            playerController.CharacterController.Move(dir * speed * Runner.DeltaTime);
            
            if (input.IsRun)
            {
                playerController.ChangeState(State.Run);
                return;
            }
            
            if (input.Horizontal != 0 || input.Vertical != 0)
                playerController.ChangeState(State.Idle);
        }
    }

    public override void StateExit()
    {

    }
}
