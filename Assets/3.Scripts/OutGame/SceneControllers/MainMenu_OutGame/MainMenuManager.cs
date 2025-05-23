using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuParticle;     // 메인메뉴 파티클 (카메라에 위치)
    
    [Header("UI Components")]
    [SerializeField] private Popup_Login popupLogin;          // 로그인 팝업 창
    [SerializeField] private GameObject GameStartButtonPanel; //게임스타트 버튼 (안보이는 화면을 다 덮는 버튼)
    [SerializeField] private GameObject SignOutButton;        //로그아웃 버튼

    public static MainMenuManager Instance;
    
    //PlayerPrefs Keys
    private const string EmailKey = "UserEmail";
    private const string PasswordKey = "UserPassword";
    
    private bool isLoginInProgress = false; //로그인 중복 시도를 막는 bool변수

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

    }
    
    public async Task AutoLogin(string email, string password)
    {
        Debug.Log($"자동 로그인 시도: {email}");
        
        if(await FirebaseAccountManager.Instance.SignIn(email, password)) //자동 로그인 성공
        {
            SignOutButton.SetActive(true); //로그아웃 버튼 활성화
        }
        else // 자동로그인 실패. PlayerPrefs는 있었지만 실제 계정이 없는 경우
        {
            PlayerPrefs.DeleteKey(EmailKey);
            PlayerPrefs.DeleteKey(PasswordKey);
        }
    }

    public void OnClickedGameStartButtonPanelButton() //화면을 클릭했을 때
    {
        // 0. 로그인 되어있다면 클릭시 씬이동
        if (FirebaseMainSession.Instance.FirebaseUser.UserData != null)
        {
            LoadingSceneManager.LoadScene("OutGame_LobbyScene");
        }
        // 1. 로그인 안되어있음 && 이전 로그인 기록이 있음 => 자동 로그인 시도
        else if (PlayerPrefs.HasKey(EmailKey) && PlayerPrefs.HasKey(PasswordKey))
        {
            Debug.Log($"자동 로그인 시도 중");
            string savedEmail = PlayerPrefs.GetString(EmailKey);
            string savedPassword = PlayerPrefs.GetString(PasswordKey);
                
            // 자동 로그인 시도
            AutoLogin(savedEmail, savedPassword);
        }
        // 3. 모두 아니라면 로그인 창을 출력한다.
        else
        {
            popupLogin.gameObject.SetActive(true);
            mainMenuParticle.SetActive(false);
        }
    }
    
    public void OnClickedSignOutButton() //화면을 덮는 버튼 위에 위치해야 한다.
    {
        if (FirebaseMainSession.Instance.FirebaseUser.UserData != null) //클릭시 세션에 로그인정보가 있으면
        {
            FirebaseAccountManager.Instance.SignOut(); //로그아웃
        }
    }

    public void ReloadMainMenuScene()
    {
        if (FirebaseMainSession.Instance.FirebaseUser.UserData != null)
        {
            SignOutButton.SetActive(true);
        }
    }
}