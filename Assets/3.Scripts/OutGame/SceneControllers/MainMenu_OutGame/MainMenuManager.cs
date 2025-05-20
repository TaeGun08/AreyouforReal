using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public enum PlayerPrefsKey
{
    UserEmail,
    UserPassword,
}

public class MainMenuManager : MonoBehaviour
{
    
    [SerializeField] private Popup_Login popups;
    [SerializeField] private AutoLoginManager autoLogin;
    
    //PlayerPrefs Keys
    private const string EmailKey = "UserEmail";
    private const string PasswordKey = "UserPassword";
    
    private void Start()
    {
        if (PlayerPrefs.HasKey(EmailKey) && PlayerPrefs.HasKey(PasswordKey))
        {
            string savedEmail = PlayerPrefs.GetString(EmailKey);
            string savedPassword = PlayerPrefs.GetString(PasswordKey);

            // 자동 로그인 시도
            autoLogin.AutoLogin(savedEmail, savedPassword);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadingSceneManager.LoadScene("LobbyScene");
        }
    }
}