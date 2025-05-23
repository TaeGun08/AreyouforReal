using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class Popup_SignUp : BaseWindow
{
    [Header("SignUp InputField")]
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField nickNameInputField;
    
    // [Space]
    // [Header("Login Panel InputField")]
    // [SerializeField] private TMP_InputField login_EmailInputField;
    // [SerializeField] private TMP_InputField login_PasswordInputField;
    
    [Space]
    [Header("PopUp Panel")]
    [SerializeField] private GameObject popupChecking;
    
    private string email = "";
    private string password = "";
    private string nickname = "";

    //PlayerPrefs Keys
    private const string EmailKey = "UserEmail";
    private const string PasswordKey = "UserPassword";
    
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
    
    public void OnClickedSignUpButtonWrapper()
    {
        _ = OnClickedSignUpButton();
    }
    
    private async Task OnClickedSignUpButton()
    {
        Debug.Log("OnClickedSignUpButton");
        email = emailInputField.text;
        password = passwordInputField.text;
        nickname = nickNameInputField.text;
        
        await FirebaseAccountManager.Instance.CreateAccount(email, password, nickname).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError(task.Exception);
                popupChecking.SetActive(true);
                return;
            }
            
            // login_EmailInputField.text = email;
            // login_PasswordInputField.text = password;
            
            PlayerPrefs.SetString(EmailKey, email);
            PlayerPrefs.SetString(PasswordKey, password);
            PlayerPrefs.Save();
            
            MainMenuManager.Instance.ReloadMainMenuScene();
            gameObject.SetActive(false);
        });
    }
}
