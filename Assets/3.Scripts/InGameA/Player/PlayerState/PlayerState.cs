using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class PlayerState : NetworkBehaviour
{
    public enum State
    {
        Idle,
        Walk,
        Run,
        Attack,
        Knockout,
    }
    
    protected PlayerController playerController;

    public abstract State CurrentState { get; }

    protected Camera mainCam;

    protected void Start()
    {
        mainCam = Camera.main;
    }

    public abstract void StateEnter(PlayerController playerController);
    public abstract void StateUpdate();
    public abstract void StateExit();
}
