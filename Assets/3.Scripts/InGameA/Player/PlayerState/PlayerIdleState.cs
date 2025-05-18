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
        if (playerController.LocalPlayer.InputJoystick())
            playerController.ChangeState(State.Walk);
    }

    public override void StateExit()
    {
    }
}