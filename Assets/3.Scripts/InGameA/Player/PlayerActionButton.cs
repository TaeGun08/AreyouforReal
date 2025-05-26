using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionButton : MonoBehaviour
{
    public static PlayerActionButton ActionButton;

    [Header("Buttons Settings")]
    [SerializeField] private ButtonHold attackButton;
    public ButtonHold AttackButton => attackButton;
    [SerializeField] private ButtonHold runButton;
    public ButtonHold RunButton => runButton;

    private void Awake()
    {
        ActionButton = this;
        
#if UNITY_EDITOR || UNITY_STANDALONE
        gameObject.SetActive(false);
#else
        gameObject.SetActive(true);
#endif
    }
    
}
