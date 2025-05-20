using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Popup_Login : BaseWindow
{
    [Header("Login Input Field")]
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    
    [Space]
    [Header("Auto Login Toggle")]
    [SerializeField] private Toggle rememberToggle;
    
    [Space]
    [Header("PopUp Panel")]
    [SerializeField] private Popup_SignUp popupSignUp;
    [SerializeField] private GameObject popupChecking;
    
    //PlayerPrefs Keys
    private const string EmailKey = "UserEmail";
    private const string PasswordKey = "UserPassword";
    
    private string email = "";
    private string password = "";
    
    //bool
    public bool isLoggedIn { get; private set; } = false;
    
    // private Regex allowedRegex = new Regex("^[a-zA-Z0-9]*$"); // 영어+숫자만 허용

    private void Awake()
    {
        emailInputField.characterLimit = 20;
        passwordInputField.characterLimit = 15;
        
        emailInputField.contentType = TMP_InputField.ContentType.Custom;
        emailInputField.onValidateInput += BlockKoreanInput;
        
        passwordInputField.contentType = TMP_InputField.ContentType.Custom;
        passwordInputField.onValidateInput += BlockKoreanInput;
    }
    
    private char BlockKoreanInput(string text, int charIndex, char addedChar)
    {
        if (IsKorean(addedChar))
        {
            return '\0'; // 입력 무시
        }
        return addedChar;
    }

    private bool IsKorean(char c)
    {
        // 한글 음절: 가 ~ 힣
        if (c >= 0xAC00 && c <= 0xD7A3) return true;

        // 한글 자모 (초성/중성/종성)
        if (c >= 0x1100 && c <= 0x11FF) return true;   // 자모 (Hangul Jamo)
        if (c >= 0x3130 && c <= 0x318F) return true;   // 호환 자모 (Compatibility Jamo)
    
        return false;
    }
    
    public void OnClickedLoginButtonWrapper()
    {
        _ = OnClickedLoginButton();
    }
    
    private async Task OnClickedLoginButton()
    {
        email = emailInputField.text;
        password = passwordInputField.text;
        
        if (await FirebaseAccountManager.Instance.SignIn(email, password)) //return bool
        {
            //로그인 성공
            isLoggedIn = true;
            
            //PlayerPrefs를 이용한 자동 로그인 세팅
            if (rememberToggle.isOn)
            {
                PlayerPrefs.SetString(EmailKey, email);
                PlayerPrefs.SetString(PasswordKey, password);
                PlayerPrefs.Save();
            }
            
            //자동 로그인 선택여부에 따라 수정
            // else
            // {
            //     PlayerPrefs.DeleteKey(EmailKey);
            //     PlayerPrefs.DeleteKey(PasswordKey);
            // }
            
            LoadingSceneManager.LoadScene("OutGame_LobbyScene");
        }
        else
        {
            //로그인 실패
            popupChecking.SetActive(true);
        }
    }
    
    public void OnClickedSignUpButton()
    {
        popupSignUp.gameObject.SetActive(true);
    }
    
    public void OnClickedForgotPasswordButton()
    {
        //later...
    }
}
