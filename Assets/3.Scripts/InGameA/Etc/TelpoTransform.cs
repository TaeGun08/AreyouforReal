using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class TelpoTransform : NetworkBehaviour
{
    public static TelpoTransform Instance;

    [SerializeField] private Transform[] telpoTrs;
    public Transform[] TelepoTrs =>  telpoTrs;
    
    private void Awake()
    {
        Instance = this;
    }
}
