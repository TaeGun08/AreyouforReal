using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static Player LocalPlayer;
    
    protected Animator animator;
    
    protected ChangeDetector changeDetector;

    [System.Serializable]
    public class PlayerStats
    {
        public int WalkSpeed;
        public int RunSpeed;
    }
    
    [Header("Character Stats")] 
    [SerializeField] protected PlayerStats stats;
    public PlayerStats Stats => stats;
}
