using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class LocalPlayer : Player
{
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
}