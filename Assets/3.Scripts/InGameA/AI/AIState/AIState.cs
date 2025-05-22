using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class AIState : NetworkBehaviour
{
    public enum State
    {
        Idle,
        Walk,
        Knockout,
    }
    
    protected AIController aiController;
    
    public abstract State CurrentState { get; }

    public abstract void StateEnter(AIController aiController);
    public abstract void StateUpdate();
    public abstract void StateExit();
}
