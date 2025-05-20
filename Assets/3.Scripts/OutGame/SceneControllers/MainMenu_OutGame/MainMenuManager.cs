using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    MainMenuManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private InputAction testAction;

    [System.Serializable]
    public class Popups
    {
        public Popup_Login logInPopup;
        // public GameObject signupPopup;
    }
    
    public Popups popups;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadingSceneManager.LoadScene("LobbyScene");
        }
    }
}