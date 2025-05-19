using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static Player LocalPlayer;
    
    protected Joystick joystick;
    
    protected NetworkCharacterController characterController;
    protected CharacterController characterControllerTest;
    protected Animator animator;

    [System.Serializable]
    public class PlayerStats
    {
        public int WalkSpeed;
        public int RunSpeed;
    }
    
    [Header("Character Stats")] 
    [SerializeField] protected PlayerStats stats;
    
    protected void Awake()
    {
        characterController = GetComponent<NetworkCharacterController>();
        characterControllerTest = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        joystick = Joystick.Instance;
    }
}
