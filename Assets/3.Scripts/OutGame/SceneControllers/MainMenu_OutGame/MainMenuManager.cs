using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Popup_Login popupLogin;
    
    //PlayerPrefs Keys
    private const string EmailKey = "UserEmail";
    private const string PasswordKey = "UserPassword";
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (PlayerPrefs.HasKey(EmailKey) && PlayerPrefs.HasKey(PasswordKey))
            {
                string savedEmail = PlayerPrefs.GetString(EmailKey);
                string savedPassword = PlayerPrefs.GetString(PasswordKey);

                // 자동 로그인 시도
                AutoLogin(savedEmail, savedPassword);
            }
            else
            {
                popupLogin.gameObject.SetActive(true);
            }
        }
        
        if (popupLogin.isLoggedIn)
        {
            LoadingSceneManager.LoadScene("OutGame_LobbyScene");
        }
    }
    
    public async Task AutoLogin(string email, string password)
    {
        Debug.Log($"자동 로그인 시도: {email}");
        
        if(await FirebaseAccountManager.Instance.SignIn(email, password))
        {
            LoadingSceneManager.LoadScene("OutGame_LobbyScene");
        }
    }
}