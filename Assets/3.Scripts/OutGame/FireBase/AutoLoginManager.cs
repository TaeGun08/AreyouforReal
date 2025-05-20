using UnityEngine;
using UnityEngine.UI;

public class AutoLoginManager : MonoBehaviour
{
    [Header("UI Elements")]
    public InputField emailInputField;
    public InputField passwordInputField;
    public Toggle rememberMeToggle;
    public Button loginButton;

    private const string EmailKey = "UserEmail";
    private const string PasswordKey = "UserPassword";

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        
        if (PlayerPrefs.HasKey(EmailKey) && PlayerPrefs.HasKey(PasswordKey))
        {
            string savedEmail = PlayerPrefs.GetString(EmailKey);
            string savedPassword = PlayerPrefs.GetString(PasswordKey);

            emailInputField.text = savedEmail;
            passwordInputField.text = savedPassword;

            // 자동 로그인 시도
            AutoLogin(savedEmail, savedPassword);
        }
    }

    void OnLoginButtonClicked()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        if (rememberMeToggle.isOn)
        {
            PlayerPrefs.SetString(EmailKey, email);
            PlayerPrefs.SetString(PasswordKey, password);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.DeleteKey(EmailKey);
            PlayerPrefs.DeleteKey(PasswordKey);
        }
        
        //AutoLogin
        FirebaseAccountManager.Instance.SignIn(email, password);
    }

    void AutoLogin(string email, string password)
    {
        Debug.Log($"자동 로그인 시도: {email}");
        FirebaseAccountManager.Instance.SignIn(email, password);
    }
}