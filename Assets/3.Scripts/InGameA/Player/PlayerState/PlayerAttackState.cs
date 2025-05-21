using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public override State CurrentState => State.Attack;
    
    public override void StateEnter(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public override void StateUpdate()
    {
        playerController.ChangeState(State.Idle);
    }

    public override void StateExit()
    {

    }
}
