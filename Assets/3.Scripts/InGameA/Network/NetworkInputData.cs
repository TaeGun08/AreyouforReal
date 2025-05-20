using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public float Horizontal;
    public float Vertical;
    
    public bool IsRun;
    public bool IsAttack;
}
