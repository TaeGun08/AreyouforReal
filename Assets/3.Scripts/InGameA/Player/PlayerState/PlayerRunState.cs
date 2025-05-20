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
        if (GetInput(out NetworkInputData input))
        {
            if (input.IsAttack)
            {
                playerController.ChangeState(State.Attack);
                return;
            }
            
            float speed = playerController.LocalPlayer.Stats.RunSpeed;
            Vector3 dir = new Vector3(input.Horizontal, 0, input.Vertical).normalized;
            playerController.CharacterController.Move(dir * speed * Runner.DeltaTime);
            
            if (input.IsRun == false)
            {
                playerController.ChangeState(State.Walk);
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
