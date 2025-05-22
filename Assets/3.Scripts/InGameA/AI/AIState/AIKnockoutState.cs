using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIKnockoutState : AIState
{
    public override State CurrentState => State.Knockout;
    
    public override void StateEnter(AIController aiController)
    {
        this.aiController = aiController;
        this.aiController.ChangeAnimation(CurrentState);
    }

    public override void StateUpdate()
    {
    }

    public override void StateExit()
    {
    }
}
