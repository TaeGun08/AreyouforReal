using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Login : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;

    [SerializeField] private Toggle rememberToggle;
    /*[System.Serializable]
    public class Buttons
    {
        public Button loginButton;
        public Button signUpButton;
        public Button forgotPasswordButton;
        public Toggle rememberButton;
        public Button exitButton;
    }
    
    public Buttons buttons;*/
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickedLoginButton()
    {
        if (rememberToggle.isOn)
        {
            //PlayerPrefs저장
        }
    }
    
    public void OnClickedSignUpButton()
    {
        
    }
    
    public void OnClickedForgotPasswordButton()
    {
        
    }
    
    public void OnClickedExitButton()
    {
        
    }
}
