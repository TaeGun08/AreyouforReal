using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CheckTexts
{
    Create,
    Join,
    AddFriend,
    AddFriendSuccess,
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
    [Header("Player Information")]
    [SerializeField] private TMP_Text playerNameText;  //플레이어 이름 표시
    
    // 경고 메시지 캐싱
    private const string CANNOT_CREATE_TEXT = "Sorry, you cannot Create the room.";
    private const string CANNOT_JOIN_TEXT = "Sorry, you cannot join the room."; 
    private const string CANNOT_ADD_FRIEND_TEXT = "Sorry, The specified user does not exist."; 
    private const string SUCCESSFUL_FRIEND_ADD_TEXT = "Successfully added friend.";
    
    public static LobbyManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(FirebaseMainSession.Instance != null)
            LobbyUpdate();
        
        // 수신 리스너 설정
        FirebaseInviteManager.Instance.ListenToInvitations(FirebaseMainSession.Instance.FirebaseUser.UserData.UserId, (inviteId, data) =>
        {
            Debug.Log($"[초대] from: {data.From}, room: {data.RoomId}");

            // UI로 수락/거절 버튼 제공
            
            FirebaseInviteManager.Instance.RespondToInvitation(inviteId, InvitationStatus.Accepted);
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
            _ => checkText.text
        };

        popupRoomList.gameObject.SetActive(true);
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
