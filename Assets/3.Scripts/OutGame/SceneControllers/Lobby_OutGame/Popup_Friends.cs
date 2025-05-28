using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class Popup_Friends : BaseWindow
{
    [Header("Friends")]
    [SerializeField] private GameObject friendListParent;
    [SerializeField] private Friend friendPrefab;
    
    [Header("Popup_AddFriend")]
    [SerializeField] private GameObject addFriendPanel;
    [SerializeField] private TMP_InputField friendKeyInputField;
    
    private List<Friend> friendList;
    private string myId;
    
    private void Awake()
    {
        friendList = new List<Friend>();
        friendList.AddRange(friendListParent.GetComponentsInChildren<Friend>(true)); // 비활성 포함하여 풀링
    }

    private void Start()
    {
        myId = FirebaseMainSession.Instance.FirebaseUser.UserData.UserId;
    }

    public void OnClickedAddFriendPanelButton() //친구 추가 팝업 띄우기
    {
        addFriendPanel.SetActive(true);
    }
    
    public void OnClickedAddFriendEnterButton() //친구 추가 확인 버튼 클릭
    {
        //인풋필드에 있는 키(친구의 키)가 존재하는지 확인
        FirestoreManager.Instance.ReadDataAsync<PlayerData>(FirebaseCollections.Players, friendKeyInputField.text).ContinueWithOnMainThread(
            task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    LobbyManager.Instance.OnPopupChecking(CheckTexts.AddFriend); //친구추가 실패 팝업
                    return;
                }
                // PlayerData friendData = task.Result;
                
                object[] friendObj = { friendKeyInputField.text };
                
                //obj형태로 배열을 업데이트
                FirestoreManager.Instance.UpdateArrayDataAsync(FirebaseCollections.Players, myId, "Friends", friendObj).ContinueWithOnMainThread(
                    task1 =>
                    {
                        addFriendPanel.SetActive(false);
                        LobbyManager.Instance.OnPopupChecking(CheckTexts.AddFriendSuccess); //친구추가 성공 팝업
                    });
            });
    }
    
    private void OnEnable()
    {
        _ = OnFriendsPopup();
    }
    
    private async Task OnFriendsPopup()
    {
        //비동기로 모든 Player를 읽어들임
        PlayerData playerData = await FirestoreManager.Instance.ReadDataAsync<PlayerData>(
                FirebaseCollections.Players,
                FirebaseMainSession.Instance.FirebaseUser.UserData.UserId);
        
        List<string> friendsKeyList = playerData.Friends;
                    
        // 부족한 개수만큼 풀링 추가
        if (friendsKeyList.Count > friendList.Count)
        {
            CreateRankInstances(playerData.Friends.Count - friendList.Count);
        }
        
        string myUserId = FirebaseMainSession.Instance.FirebaseUser.UserData.UserId;
        int friendIndex = 0;
        
        if (friendList.Count > 0)
        {
            foreach (var friendKey in friendsKeyList)
            {
                //비동기로 Friend를 읽어들임
                PlayerData friendData = await FirestoreManager.Instance.ReadDataAsync<PlayerData>(FirebaseCollections.Players, friendKey);
                
                friendList[friendIndex].SetFriend(friendKey, friendData.NickName);
                friendList[friendIndex].gameObject.SetActive(true);
                friendIndex++;
            }
        }
        
        // roomIndex가 끝난 지점부터 ~ 사용하지 않는 오브젝트들을 끕니다.
        for (int i = friendIndex; i < friendList.Count; i++)
        {
            friendList[i].gameObject.SetActive(false);
        }
    }
    
    private void CreateRankInstances(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Friend friend = Instantiate(friendPrefab, friendListParent.transform);
            friend.gameObject.SetActive(false);
            friendList.Add(friend);
        }
    }
}
