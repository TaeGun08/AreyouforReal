using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.Services.Vivox;
using UnityEngine;

public class VivoxTestScript : NetworkBehaviour
{
    private async void Start()
    {
        await VivoxManager.Instance.Init();
        await VivoxManager.Instance.Join3DChannel(gameObject, "Global");
        _ = VivoxService.Instance.SetChannelTransmissionModeAsync(TransmissionMode.All);
    }

    public override void FixedUpdateNetwork()
    {
        if (VivoxManager.Instance.isInChannel)
        {
            VivoxManager.Instance.Update3DPosition(gameObject, "Global");
        }
    }
}
