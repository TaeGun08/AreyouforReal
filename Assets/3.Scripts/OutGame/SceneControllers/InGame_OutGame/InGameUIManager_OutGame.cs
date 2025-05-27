using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Firebase.Database;
using Firebase.Extensions;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager_OutGame : MonoBehaviour
{
    [Header("Top RoomCode UI")]
    [SerializeField] private TMP_Text roomCode;
    
    [Space]
    [Header("PopUps")]
    [SerializeField] private GameObject Popup_Chat;
    [SerializeField] private GameObject Popup_ExitChecking;
    
    [Space]
    [Header("Buttons")]
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject waitingButton;

    
    private NetworkRunner runner;
    
    public static InGameUIManager_OutGame Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        startButton.SetActive(false);
        waitingButton.SetActive(false);
        runner = FindObjectOfType<NetworkRunner>();
        roomCode.SetText(runner.SessionInfo.Name);
    }

    public void OnClickedExitButton() // 나가기 확인 팝업 출력
    {
        Popup_ExitChecking.SetActive(true);
    }
    
    public void OnClickedExitSureButton() //나가기 확인 눌림
    {
        bool isEndExit = false;
        //ToDo : 시형님 여기에 나가는 로직 추가 부탁드립니당
        
        
        //
        
        if (isEndExit)
        {
            LoadingSceneManager.LoadScene("Lobby");
        }
    }
    
    public void OnClickedStartButton() //게임 시작 버튼
    {
        GameManager_Network.Instance.TryStartGame();
        startButton.gameObject.SetActive(false);
    }
    
    public void OnClickedChatButton()
    {
        Popup_Chat.SetActive(true);
    }
    
    public void UpdateButtonState(bool isCanStart)  //시작버튼 활성화
    {
        if(!runner.IsServer) return;  //서버 아니면 날림
        
        if (isCanStart)
        {
            startButton.SetActive(true);
            waitingButton.SetActive(false);
        }
        else
        {
            startButton.SetActive(false);
            waitingButton.SetActive(true);
        }
    }

    //초대 보네기
    public void OnClickInviteSureButton()
    {
        SendInvite(FirebaseMainSession.Instance.FirebaseUser.UserData.UserId, "친구");
    }
    
    public void SendInvite(string hostUid, string guestUid)
    {
        var inviteData = new Dictionary<string, object>
        {
            { "host", hostUid },
            { "status", "pending" },
            { "timestamp", ServerValue.Timestamp }
        };

        FirebaseDatabase.DefaultInstance
            .GetReference("user_invites")
            .Child(guestUid)
            .Child(roomCode.text)
            .SetValueAsync(inviteData);
    }

}
