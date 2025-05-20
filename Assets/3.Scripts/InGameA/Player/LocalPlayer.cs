using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class LocalPlayer : Player
{
    public bool IsRun { get; set; }
    
    public override void Spawned()
    {
        changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        
        if (Object.HasStateAuthority)
        {
            PlayerRegistry.Instance.AddPlayer(Runner, Object.InputAuthority, this);
        }
        
        if (Object.HasInputAuthority)
        {
            LocalPlayer = this;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData input))
        {
            float speed = input.IsRunning ? stats.RunSpeed : stats.WalkSpeed;
            Vector3 dir = new Vector3(input.Horizontal, 0, input.Vertical).normalized;
            characterController.Move(dir * speed * Runner.DeltaTime);
        }
    }

    public bool InputJoystick()
    {
        if (GetInput(out NetworkInputData input))
        {
            if (input.Horizontal != 0 || input.Vertical != 0) return true;
        }
        
        return false;
    }

    public bool InputRun()
    {
        if (GetInput(out NetworkInputData input))
        {
            if (input.IsRunning) return true;
        }
        
        return false;
    }
}