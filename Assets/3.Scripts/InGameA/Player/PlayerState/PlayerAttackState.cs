using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public override State CurrentState => State.Attack;
    
    public override void StateEnter(PlayerController playerController)
    {
        this.playerController = playerController;
        this.playerController.ChangeAnimation(CurrentState);
    }

    public override void StateUpdate()
    {
        if (GetInput(out NetworkInputData input))
        {
            playerController.ChangeState(State.Idle);
        }
    }

    public override void StateExit()
    {
        
    }
}
