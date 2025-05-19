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
        
        PerformLogin(email, password);
    }

    void AutoLogin(string email, string password)
    {
        Debug.Log($"자동 로그인 시도: {email}");
        //FirebaseManager로 처리
        PerformLogin(email, password);
    }

    void PerformLogin(string email, string password)
    {
        // 여기에 실제 로그인 로직 작성
        // 예시: 서버 요청 or 로컬 인증 등
        // Debug.Log($"로그인 성공: {email}");
        //FirebaseManager로 처리
    }
}