using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Setting : BaseWindow
{

    [Header("UI Components")]
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    
    private const string BgmVolumeKey = "BGM_VOLUME";
    private const string SfxVolumeKey = "SFX_VOLUME";
    
    private void Start()
    {
        // 슬라이더 초기값 세팅 (저장된 값 불러오기)
        float savedVolume = PlayerPrefs.GetFloat(BgmVolumeKey, 1f);
        float savedSfx = PlayerPrefs.GetFloat(SfxVolumeKey, 1f);
        
        masterVolumeSlider.value = savedVolume;
        sfxVolumeSlider.value = savedSfx;
        
        // 슬라이더 변경 이벤트 연결
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        masterVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
    }
    
    private void OnMasterVolumeChanged(float value)
    {
        // 저장
        PlayerPrefs.SetFloat(BgmVolumeKey, value);
        PlayerPrefs.Save();
    
        // 사운드 매니저에 전달
        SoundManager.Instance.SetBgmVolume(value);
    }
    
    private void OnSfxVolumeChanged(float value)
    {
        // 저장
        PlayerPrefs.SetFloat(SfxVolumeKey, value);
        PlayerPrefs.Save();
    
        // 사운드 매니저에 전달
        SoundManager.Instance.SetSfxVolume(value);
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
