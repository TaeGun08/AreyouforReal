using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;

public class VivoxController : MonoBehaviour
{
    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        // await AuthenticationService;

    }
}
