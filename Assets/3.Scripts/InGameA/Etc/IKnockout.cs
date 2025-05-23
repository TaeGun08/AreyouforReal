using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public interface IKnockout
{
    public NetworkObject NetworkObj { get; }

    public void RPC_Knockout();
}
