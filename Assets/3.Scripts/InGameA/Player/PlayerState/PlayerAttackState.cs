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
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(1f);
        playerController.ChangeState(State.Idle);
    }

    public override void StateUpdate()
    {
    }

    public override void StateExit()
    {
        playerController.ResetAnimation(CurrentState);
    }
}
