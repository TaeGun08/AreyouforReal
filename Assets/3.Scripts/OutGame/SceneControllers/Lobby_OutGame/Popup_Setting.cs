using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Setting : BaseWindow
{

    [Header("UI Components")]
    public Slider masterVolumeSlider;
    
    private const string BgmVolumeKey = "BGM_VOLUME";
    
    private void Start()
    {
        // 슬라이더 초기값 세팅 (저장된 값 불러오기)
        float savedVolume = PlayerPrefs.GetFloat(BgmVolumeKey, 1f);

        masterVolumeSlider.value = savedVolume;
    
        // 슬라이더 변경 이벤트 연결
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
    }
    
    private void OnMasterVolumeChanged(float value)
    {
        // 저장
        PlayerPrefs.SetFloat(BgmVolumeKey, value);
        PlayerPrefs.Save();
    
        // 사운드 매니저에 전달
        SoundManager.Instance.SetBgmVolume(value);
    }
    
    public void OnClickedUserIdButton()
    {
        GUIUtility.systemCopyBuffer = FirebaseMainSession.Instance.FirebaseUser.UserData.UserId;
        LobbyManager.Instance.OnPopupChecking(CheckTexts.CopyMyId);
    }

    public void OnClickedExitGameButton()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
