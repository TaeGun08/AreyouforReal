using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionButton : MonoBehaviour
{
    public static PlayerActionButton ActionButton;

    [Header("Buttons Settings")]
    [SerializeField] private Button runButton;
    [SerializeField] private Button attackButton;
    public bool IsRun { get; private set; }
    public bool IsAttack { get; private set; }

    private void Awake()
    {
        ActionButton = this;
        OnRunButton();
        OnAttackButton();
    }

    private void OnRunButton()
    {
        runButton.onClick.AddListener(() =>
        {
            IsRun = true;
        });
    }

    private void OnAttackButton()
    {
        attackButton.onClick.AddListener(() =>
        {
            IsAttack = true;
        });
    }

    public void RunButtonUp()
    {
        IsRun = false;
    }
}
