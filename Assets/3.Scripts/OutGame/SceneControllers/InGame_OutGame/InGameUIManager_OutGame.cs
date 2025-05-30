using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Firebase.Database;
using Firebase.Extensions;
using Fusion;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InGameUIManager_OutGame : NetworkBehaviour
{
    [Header("Top RoomCode UI")]
    [SerializeField] private TMP_Text roomCode;
    [SerializeField] private GameObject roomCodePanel;
    
    [Space]
    [Header("PopUps")]
    [SerializeField] private GameObject Popup_Chat;
    [SerializeField] private GameObject Popup_ExitChecking;
    [SerializeField] private GameObject friendInvitePanel;
    
    [Space]
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject waitingButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject inviteButton;
    
    private NetworkRunner runner;
    
    public static InGameUIManager_OutGame Instance;


    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SoundManager.Instance.PlayBgm("こんとどぅふぇ素材No.0170-すすめベビーゴールデンレトリーバー");
        
        startButton.gameObject.SetActive(false);
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
        Player.LocalPlayer.Runner.Shutdown();
        LoadingSceneManager.LoadScene("Lobby");
    }

    public void OnClickedFriendInviteButton()
    {
        friendInvitePanel.SetActive(true);
    }
    
    public string GetRoomCode()
    {
        return roomCode.text;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    private void RPC_DisableStartUI()
    {
        startButton.gameObject.SetActive(false);
        waitingButton.SetActive(false);
        exitButton.SetActive(false);
        roomCodePanel.SetActive(false);
        Popup_Chat.SetActive(false);
        Popup_ExitChecking.SetActive(false);
        inviteButton.SetActive(false);
    }
    
    public void OnClickedStartButton() //게임 시작 버튼
    {
        startButton.interactable = false;
        RPC_DisableStartUI(); // 모든 클라이언트에게 UI 비활성화 전파
        
        Dictionary<string, object> updateIsGameStarted = new Dictionary<string, object> {
            { "IsGameStarted", true }
        };

        //룸 게임 시작 bool 업데이트
        FirestoreManager.Instance.UpdateDataAsync(FirebaseCollections.Rooms, roomCode.text, updateIsGameStarted)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    startButton.interactable = true;
                    return;
                }

                GameManager_Network.Instance.TryStartGame();//게임시작
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
            startButton.gameObject.SetActive(true);
            waitingButton.SetActive(false);
        }
        else
        {
            startButton.gameObject.SetActive(false);
            waitingButton.SetActive(true);
        }
    }
}
