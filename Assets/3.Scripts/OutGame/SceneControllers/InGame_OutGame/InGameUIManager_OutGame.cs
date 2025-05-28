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
    [SerializeField] private GameObject roomCodePanel;
    
    [Space]
    [Header("PopUps")]
    [SerializeField] private GameObject Popup_Chat;
    [SerializeField] private GameObject Popup_ExitChecking;
    
    [Space]
    [Header("Buttons")]
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject waitingButton;
    [SerializeField] private GameObject exitButton;
    
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
        LoadingSceneManager.LoadScene("Lobby");
    }
    
    public void OnClickedStartButton() //게임 시작 버튼
    {
        startButton.SetActive(false);
        waitingButton.SetActive(false);
        exitButton.SetActive(false);
        roomCodePanel.SetActive(false);
        Popup_Chat.SetActive(false);
        Popup_ExitChecking.SetActive(false);
        
        Dictionary<string, object> updateIsGameStarted =  new Dictionary<string, object>{
                {"IsGameStarted" , true}
            };

        //룸 게임 시작 bool 업데이트
        FirestoreManager.Instance.UpdateDataAsync(FirebaseCollections.Rooms, roomCode.text, updateIsGameStarted)
            .ContinueWithOnMainThread(
                task =>
                {
                    if(task.IsFaulted ||  task.IsCanceled) return;
                    
                    GameManager_Network.Instance.TryStartGame();  //게임시작
                });
    }
    
    public void OnClickedChatButton()
    {
        Popup_Chat.SetActive(true);
    }
    
    public void UpdateButtonState(bool isCanStart)  //시작버튼 활성화
    {
        if(!runner.IsSharedModeMasterClient) return;  //서버 아니면 날림
        
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
    public void OnClickInviteSureButton() //친구창의 친구 옆에 표시할 초대 버튼으로
    {
        FirebaseInviteManager.Instance.SendInvitation(
            FirebaseMainSession.Instance.FirebaseUser.UserData.UserId, //from
            "Friend",   //to (Friend)
            roomCode.text //roomCode
            );
    }
}
