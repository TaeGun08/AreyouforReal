using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CheckTexts
{
    Create,
    Join,
    AddFriend,
    AddFriendSuccess,
    CopyMyId,
}

public class LobbyManager : MonoBehaviour
{

    [Header("PopUps")]
    [SerializeField] private Popup_RoomList popupRoomList; // 방 목록 표시 팝업
    [SerializeField] private GameObject popupFriend;       // 친구 목록 표시 팝업 (임시)
    // [SerializeField] private Popup_Chat popupChat;         // 채팅 목록 표시 팝업 (임시)
    [SerializeField] private GameObject popupRanking;      // 랭킹 표시 팝업 (임시)
    [SerializeField] private GameObject popupSetting;      // 세팅 표시 팝업 (임시)
    [SerializeField] private GameObject popupChecking;     // 경고 팝업
    [SerializeField] private TMP_Text checkText;           // 경고문 텍스트
    
    [Space]
    [Header("PopUp_Invite")]
    [SerializeField] private GameObject popupChecking_Invite;       // 초대 팝업
    [SerializeField] private Button popupChecking_EnterButton;      // 확인 버튼
    [SerializeField] private Button popupChecking_CancelButton;     // 캔슬 버튼
    [SerializeField] private TMP_Text checkText_Invite;             // 초대문 텍스트
    
    [Space]
    [Header("Player Information")]
    [SerializeField] private TMP_Text playerNameText;  //플레이어 이름 표시
    
    // 경고 메시지 캐싱
    private const string CANNOT_CREATE_TEXT = "Sorry, you cannot Create the room.";
    private const string CANNOT_JOIN_TEXT = "Sorry, you cannot join the room."; 
    private const string CANNOT_ADD_FRIEND_TEXT = "Sorry, The specified user does not exist."; 
    private const string SUCCESSFUL_FRIEND_ADD_TEXT = "Successfully added friend.";
    private const string COPY_MY_ID_TEXT = "Your ID has been successfully copied to the clipboard.";
    
    public static LobbyManager Instance;

    private string inviteIdCache;
    private string inviteRoomCodeCache;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SoundManager.Instance.PlayBgm("こんとどぅふぇ素材No.0173-冬眠終了！");
        
        if(FirebaseMainSession.Instance != null)
            LobbyUpdate();
        
        // 수신 리스너 설정
        FirebaseInviteManager.Instance.ListenToInvitations(FirebaseMainSession.Instance.FirebaseUser.UserData.UserId, (inviteId, data) =>
        {
            Debug.Log($"[초대] from: {data.From}, room: {data.RoomId}");

            // UI로 수락/거절 버튼 제공
            FirestoreManager.Instance.ReadDataAsync<PlayerData>(FirebaseCollections.Players, data.From).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    return;
                }
                
                PlayerData friendData = task.Result;
                popupChecking_EnterButton.onClick.AddListener(ConfirmInviteRoom);
                popupChecking_CancelButton.onClick.AddListener(CancelInviteRoom);
                checkText_Invite.text = $"You have received an invitation from {friendData.NickName}.";
                popupChecking_Invite.SetActive(true);
                
            });
            inviteRoomCodeCache = data.RoomId;
            inviteIdCache = inviteId;
        });
    }

    public void ConfirmInviteRoom()
    {
        popupChecking_Invite.SetActive(false);
        
        //초대 응답 firestore
        FirebaseInviteManager.Instance.RespondToInvitation(inviteIdCache, InvitationStatus.Accepted).ContinueWithOnMainThread(
            task =>
            {
                if(task.IsFaulted || task.IsCanceled) return;
                _ = NetworkStartBridge.Instance.JoinRoom(inviteRoomCodeCache); // 입장 가능 검사를 JoinRoom에서 하도록 수정
            });
    }
    
    public void CancelInviteRoom()
    {
        popupChecking_Invite.SetActive(false);
        
        //초대 거절 firestore
        FirebaseInviteManager.Instance.RespondToInvitation(inviteIdCache, InvitationStatus.Declined).ContinueWithOnMainThread(
            task =>
            {
                OnPopupChecking(CheckTexts.Join); //입장 실패 경고문
            });
    }
    
    void OnDisable()
    {
        FirebaseInviteManager.Instance.StopListening();
    }
    
    private void LobbyUpdate()
    {
        playerNameText.text = FirebaseMainSession.Instance.FirebaseUser.Username;
    }

    public void OnPopupChecking( CheckTexts checkEnum )   //경고 팝업 표시
    {
        checkText.text = checkEnum switch
        {
            CheckTexts.Create => CANNOT_CREATE_TEXT,
            CheckTexts.Join => CANNOT_JOIN_TEXT,
            CheckTexts.AddFriend => CANNOT_ADD_FRIEND_TEXT,
            CheckTexts.AddFriendSuccess => SUCCESSFUL_FRIEND_ADD_TEXT,
            CheckTexts.CopyMyId => COPY_MY_ID_TEXT,
            _ => checkText.text
        };

        popupChecking.gameObject.SetActive(true);
    }
    
    public void OnClickedRoomListButton() //방 들어가는 버튼
    {
        popupRoomList.gameObject.SetActive(true);
    }
    
    public void OnClickedFriendButton() // 친구 창 팝업 버튼
    {
        popupFriend.gameObject.SetActive(true);
    }
    
    public void OnClickedChatButton() // 채팅 팝업 버튼
    {
        // popupChat.gameObject.SetActive(true);
    }
    
    public void OnClickedRankingButton() // 랭킹 팝업 버튼
    {
        popupRanking.gameObject.SetActive(true);
    }
    
    public void OnClickedSettingButton() // 세팅 팝업 버튼
    {
        popupSetting.gameObject.SetActive(true);
    }
}
