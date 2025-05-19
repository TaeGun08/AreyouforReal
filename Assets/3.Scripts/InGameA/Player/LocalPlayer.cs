using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : Player
{
    public bool IsRun { get; set; }

    protected override void Start()
    {
        base.Start();
        LocalPlayer = this;
    }

    private void FixedUpdate()
    {
        characterControllerTest.Move(new Vector3(joystick.Horizontal,
            0f, joystick.Vertical).normalized * (IsRun ? stats.RunSpeed : stats.WalkSpeed * Time.fixedDeltaTime));
    }

    // public override void FixedUpdateNetwork()
    // {
    //     if (GetInput(out NetworkInputData input))
    //     {
    //         float speed = input.IsRunning ? stats.RunSpeed : stats.WalkSpeed;
    //         Vector3 dir = new Vector3(input.Horizontal, 0, input.Vertical).normalized;
    //         characterController.Move(dir * speed * Runner.DeltaTime);
    //     }
    // }

    // public bool InputJoystick()
    // {
    //     if (GetInput(out NetworkInputData input))
    //     {
    //         if (input.Horizontal != 0 || input.Vertical != 0) return true;
    //     }
    //     
    //     return false;
    // }

    // public bool InputRun()
    // {
    //     if (GetInput(out NetworkInputData input))
    //     {
    //         if (input.IsRunning) return true;
    //     }
    //     
    //     return false;
    // }

    public bool InputJoystick()
    {
        if (joystick.Horizontal != 0 || joystick.Vertical != 0) return true;

        return false;
    }

    // public bool InputRun()
    // {
    //     if (input.IsRunning) return true;
    //
    //     return false;
    // }
}