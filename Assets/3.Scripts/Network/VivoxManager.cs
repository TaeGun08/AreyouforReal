using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;
using UnityEngine;

[Serializable]
public class Channel3DSetting
{
    //가청거리
    [SerializeField] private int audibleDistance = 32;

    //작아지기 시작하는 거리
    [SerializeField] private int conversationalDistance = 1;

    //FadeModel에 따른 감쇠 강도
    [SerializeField] private float audioFadeIntensityByDistance = 1.0f;

    //위치에따른 음량 감쇠 모델
    [SerializeField] private AudioFadeModel audioFadeModel = AudioFadeModel.InverseByDistance;

    public Channel3DProperties GetChannel3DProperties()
    {
        return new Channel3DProperties(audibleDistance, conversationalDistance, audioFadeIntensityByDistance, audioFadeModel);
    }
}

public class VivoxManager : MonoBehaviour
{
    public static VivoxManager Instance { get; private set; }
    
    [Header("3D Channel Settings")]
    [SerializeField] private Channel3DSetting channel3DSetting;
    
    private async void Awake()
    {
        // 필수
        // Unity 계정 인증(Authentication) 
        // Vivox 서비스 초기화(InitializeAsync)
        // Vivox 사용자 로그인(LoginAsync)
        
        Instance = this;
    }

    public async Task Init()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        await VivoxService.Instance.InitializeAsync();
        Debug.Log("VivoxService 초기화 완료");
        
        await LoginAsync();
        Debug.Log("Vivox 로그인 완료");

        // await JoinVoiceChannel("Global");
    }
    
    private async Task LoginAsync()
    {
        var options = new LoginOptions
        {
            DisplayName = Guid.NewGuid().ToString()
        };

        //로그인
        await VivoxService.Instance.LoginAsync(options);
    }

    // 채널이 없으면 자동으로 생성
    public async Task JoinVoiceChannel(string channelName)
    {
        await VivoxService.Instance.JoinGroupChannelAsync(channelName, ChatCapability.AudioOnly);
    }
    
    /// <summary>
    /// Player에서 조인
    /// </summary>
    /// <param name="speakObj"></param>
    /// <param name="channelName"></param>
    public async Task Join3DChannel(GameObject speakObj, string channelName)
    {
        //위치 음성 채널에 접속
        await VivoxService.Instance.JoinPositionalChannelAsync(channelName, ChatCapability.AudioOnly, channel3DSetting.GetChannel3DProperties());
    }

    /// <summary>
    /// FixedNetwork에서 호출
    /// </summary>
    /// <param name="speakeObj"></param>
    /// <param name="channelName"></param>
    public void Update3DPosition(GameObject speakeObj, string channelName)
    {
        //위치를 업데이트
        VivoxService.Instance.Set3DPosition(speakeObj, channelName);
    }
}
