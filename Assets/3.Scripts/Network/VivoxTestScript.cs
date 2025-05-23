using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class VivoxTestScript : NetworkBehaviour
{
    private async void Start()
    {
        await VivoxManager.Instance.Init();
        _ = VivoxManager.Instance.Join3DChannel(gameObject, "Global");
    }

    public override void FixedUpdateNetwork()
    {
        VivoxManager.Instance.Update3DPosition(gameObject, "Global");
    }
}
