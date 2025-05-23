using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [Header("PopUps")]
    [SerializeField] private Popup_RoomList popupRoomList; // 방 목록 표시 팝업
    [SerializeField] private GameObject popupFriend;       // 친구 목록 표시 팝업 (임시)
    [SerializeField] private Popup_Chat popupChat;         // 채팅 목록 표시 팝업 (임시)
    [SerializeField] private GameObject popupRanking;      // 랭킹 표시 팝업 (임시)
    [SerializeField] private GameObject popupSetting;      // 세팅 표시 팝업 (임시)
    
    [Space]
    [Header("Player Information")]
    [SerializeField] private TMP_Text playerNameText;
    

    private void Start()
    {
        throw new NotImplementedException();
    }

    private void LobbyUpdate()
    {
        // playerNameText.text = FirebaseMainSession.Instance.FirebaseUser.Username;
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
