using System;
using System.Collections;
using System.Collections.Generic;
using _3.Scripts.Network;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.Events;

public class VoiceManager : MonoBehaviour
{
    public const byte GLOBAL = 0;

    public static VoiceManager Instance { get; private set; }
    public VoiceConnection VoiceConn { get; private set; }
    public Recorder VoiceRec { get; private set; }
    public MicrophoneSelector MicrophoneSelector { get; private set; }

    [field: SerializeField] public UnityEvent OnVoiceInit { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void Init(VoiceConnection voiceConn, Recorder voiceRec)
    {
        VoiceConn = voiceConn;
        VoiceRec = voiceRec;

        VoiceRec.MicrophoneDevice = MicrophoneSelector.CurrentDevice;
        if (OnVoiceInit != null) OnVoiceInit.Invoke();
    }

    public void JoinListenChannel(byte ch)
    {
        VoiceConn.Client.OpChangeGroups(null, new byte[] { ch });
    }

    public void ClearListenChannel(byte ch)
    {
        VoiceConn.Client.OpChangeGroups(new byte[] { ch }, null);
    }

    public void SetTalkChannel(byte ch)
    {
        VoiceRec.InterestGroup = ch;
    }
}
