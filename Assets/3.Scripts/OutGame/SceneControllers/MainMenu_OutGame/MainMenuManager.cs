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
    
    private bool isLoginInProgress = false; //로그인 중복 시도를 막는 bool변수
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isLoginInProgress.Equals(false)) // any pressed
        {
            if (PlayerPrefs.HasKey(EmailKey) && PlayerPrefs.HasKey(PasswordKey)) // 자동 로그인 트리거
            {
                isLoginInProgress = true; //중복실행을 막음
                
                string savedEmail = PlayerPrefs.GetString(EmailKey);
                string savedPassword = PlayerPrefs.GetString(PasswordKey);

                // 자동 로그인 시도
                AutoLogin(savedEmail, savedPassword);
            }
            else // 자동 로그인 실패
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