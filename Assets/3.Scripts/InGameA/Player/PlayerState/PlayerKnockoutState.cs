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

        GameResult.Instance.RecordElimination(this.playerController);
        
        StartCoroutine(RecordSceneLoadCoroutine());
    }

    private IEnumerator RecordSceneLoadCoroutine()
    {
        yield return new WaitForSeconds(3f);
        playerController.Runner.Shutdown();
        SceneManager.LoadSceneAsync(4);
    }

    public override void StateUpdate()
    {
    }

    public override void StateExit()
    {
    }
}
