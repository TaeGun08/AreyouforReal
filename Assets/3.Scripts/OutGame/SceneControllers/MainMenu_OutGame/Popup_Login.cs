using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Popup_Login : BaseWindow
{
    [Header("User Input Field")]
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    
    [Space]
    [Header("Auto Login Toggle")]
    [SerializeField] private Toggle rememberToggle;
    
    [Space]
    [Header("PopUp Panel")]
    [SerializeField] private Popup_SignUp signUpPopUp;
    [SerializeField] private GameObject popupChecking;
    
    //PlayerPrefs Keys
    private const string EmailKey = "UserEmail";
    private const string PasswordKey = "UserPassword";
    
    //bool
    private bool isLoggedIn = false;
    private bool isSignUpMode = false;
    

    
    public void OnClickedLoginButton()
    {
        //캐싱
        string email = emailInputField.text;
        string password = passwordInputField.text;
        
        if (FirebaseAccountManager.Instance.SignIn(email, password)) //return bool
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
            
            LoadingSceneManager.LoadScene("LobbyScene");
        }
        else
        {
            //로그인 실패
            popupChecking.SetActive(true);
        }
    }
    
    public void OnClickedSignUpButton()
    {
        signUpPopUp.gameObject.SetActive(true);
    }
    
    public void OnClickedForgotPasswordButton()
    {
        //later...
    }
}
