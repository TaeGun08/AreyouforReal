using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class AIKnockoutState : AIState
{
    public override State CurrentState => State.Knockout;
    
    public override void StateEnter(AIController aiController)
    {
        this.aiController = aiController;
        this.aiController.ChangeAnimation(CurrentState);
<<<<<<< HEAD
        StartCoroutine(RecoveryCoroutine());
    }

    private IEnumerator RecoveryCoroutine()
    {
        yield return new WaitForSeconds(3f);
        aiController.ChangeState(State.Idle);
=======
        // this.aiController.Runner.Despawn(this.aiController.Object);
>>>>>>> Develop_Network_Shared
    }

    public override void StateUpdate()
    {
    }

    public override void StateExit()
    {
    }
}
