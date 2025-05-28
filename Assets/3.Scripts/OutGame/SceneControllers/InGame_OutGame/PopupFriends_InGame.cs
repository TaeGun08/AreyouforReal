using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class PopupFriends_InGame : BaseWindow
{
    [Header("Friends")]
    [SerializeField] private GameObject friendListParent;
    [SerializeField] private Friend_Invite friendPrefab_Invite;
    
    private List<Friend_Invite> friendList;
    private string myId;

    private void Awake()
    {
        friendList = new List<Friend_Invite>();
        friendList.AddRange(friendListParent.GetComponentsInChildren<Friend_Invite>(true)); // 비활성 포함하여 풀링
        myId = FirebaseMainSession.Instance.FirebaseUser.UserData.UserId;
    }
    
    private void OnEnable()
    {
        _ = OnFriendsPopup();
    }
    
    private async Task OnFriendsPopup()
    {
        //비동기로 Player를 읽어들임
        PlayerData playerData = await FirestoreManager.Instance.ReadDataAsync<PlayerData>(
            FirebaseCollections.Players,
            myId);

        List<string> friendsKeyList = playerData.Friends;
        
        // 부족한 개수만큼 풀링 추가
        if (friendsKeyList.Count > friendList.Count)
        {
            CreateFriendInstances(playerData.Friends.Count - friendList.Count);
        }
        
        int friendIndex = 0;
        
        if (friendsKeyList.Count > 0)
        {
            foreach (var friendKey in friendsKeyList)
            {
                //비동기로 Friend를 읽어들임
                PlayerData friendData = await FirestoreManager.Instance.ReadDataAsync<PlayerData>(FirebaseCollections.Players, friendKey);
                
                friendList[friendIndex].SetFriend( friendData.NickName, friendKey );
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
    
    private void CreateFriendInstances(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Friend_Invite friend = Instantiate(friendPrefab_Invite, friendListParent.transform);
            friend.gameObject.SetActive(false);
            friendList.Add(friend);
        }
    }
}
