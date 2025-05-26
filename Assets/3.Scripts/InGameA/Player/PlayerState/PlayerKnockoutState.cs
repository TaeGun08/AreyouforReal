using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerKnockoutState : PlayerState
{
    public override State CurrentState => State.Knockout;
    
    public override void StateEnter(PlayerController playerController)
    {
        this.playerController = playerController;
        this.playerController.ChangeAnimation(CurrentState);

        GameManager_Network.Instance.KillEvent(this.playerController.LocalPlayer);
        // 퇴장 기록
        //GameResult.Instance.RecordElimination(playerController);
        SceneManager.LoadScene(2);
    }

    public override void StateUpdate()
    {
    }

    public override void StateExit()
    {
    }
}
