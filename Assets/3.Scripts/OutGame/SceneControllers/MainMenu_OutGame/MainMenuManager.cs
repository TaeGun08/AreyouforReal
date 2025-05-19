using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private InputAction testAction;


    private void OnEnable()
    {
        testAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/space");
        testAction.Enable();
        testAction.performed += LoadLobbyScene;
    }
    
    public void LoadLobbyScene(InputAction.CallbackContext context)
    {
        LoadingSceneManager.LoadScene("LobbyScene");
    }
}