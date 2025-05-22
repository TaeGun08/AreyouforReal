using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockoutState : PlayerState
{
    public override State CurrentState => State.Knockout;
    
    public override void StateEnter(PlayerController playerController)
    {
        this.playerController = playerController;
        this.playerController.ChangeAnimation(CurrentState);

        // 퇴장 기록
        GameResult.Instance.RecordElimination(playerController);
    }

    public override void StateUpdate()
    {
    }

    public override void StateExit()
    {
    }
}
