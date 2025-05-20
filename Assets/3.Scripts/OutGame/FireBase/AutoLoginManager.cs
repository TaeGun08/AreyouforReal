using UnityEngine;
using UnityEngine.UI;

public class AutoLoginManager : MonoBehaviour
{
    [Header("UI Elements")]
    public InputField emailInputField;
    public InputField passwordInputField;
    public Toggle rememberMeToggle;

    private const string EmailKey = "UserEmail";
    private const string PasswordKey = "UserPassword";

    void Start()
    {
        

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

    public void AutoLogin(string email, string password)
    {
        Debug.Log($"자동 로그인 시도: {email}");
        FirebaseAccountManager.Instance.SignIn(email, password);
    }
}