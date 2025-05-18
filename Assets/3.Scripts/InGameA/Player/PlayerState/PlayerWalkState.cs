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
    }

    public override void StateUpdate()
    {
        if (playerController.LocalPlayer.InputJoystick() == false)
            playerController.ChangeState(State.Idle);
    }

    public override void StateExit()
    {

    }
}
