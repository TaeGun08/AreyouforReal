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
}

public class LobbyManager : MonoBehaviour
{

    [Header("PopUps")]
    [SerializeField] private Popup_RoomList popupRoomList; // 방 목록 표시 팝업
    [SerializeField] private GameObject popupFriend;       // 친구 목록 표시 팝업 (임시)
    [SerializeField] private Popup_Chat popupChat;         // 채팅 목록 표시 팝업 (임시)
    [SerializeField] private GameObject popupRanking;      // 랭킹 표시 팝업 (임시)
    [SerializeField] private GameObject popupSetting;      // 세팅 표시 팝업 (임시)
    [SerializeField] private GameObject popupChecking;     // 경고 팝업
    [SerializeField] private TMP_Text checkText;           // 경고문 텍스트
    
    [Space]
    [Header("Player Information")]
    [SerializeField] private TMP_Text playerNameText;  //플레이어 이름 표시
    
    // 경고 메시지 캐싱
    private const string cannotCreateText = "Sorry, you cannot Create the room.";
    private const string cannotJoinText = "Sorry, you cannot join the room."; 

    
    public static LobbyManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(FirebaseMainSession.Instance != null)
            LobbyUpdate();
    }

    private void LobbyUpdate()
    {
        playerNameText.text = FirebaseMainSession.Instance.FirebaseUser.Username;
    }

    public void OnPopupChecking( CheckTexts checkEnum )   //경고 팝업 표시
    {

        if (checkEnum == CheckTexts.Create)
        {
            checkText.text =  cannotCreateText;
        }
        else if (checkEnum == CheckTexts.Join)
        {
            checkText.text = cannotJoinText;
        }

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
        popupChat.gameObject.SetActive(true);
    }
    
    public void OnClickedRankingButton() // 랭킹 팝업 버튼
    {
        popupRanking.gameObject.SetActive(true);
    }
    
    public void OnClickedSettingButton() // 세팅 팝업 버튼
    {
        popupRanking.gameObject.SetActive(true);
    }
}
