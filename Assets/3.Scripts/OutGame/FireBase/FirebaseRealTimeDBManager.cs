using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseRealTimeDBManager : MonoBehaviour
{
    private void Update()
    {
        
    }
    
    public void ListenForInvites(string myUid, Action<string, string> onInvite)
    {
        var refPath = FirebaseDatabase.DefaultInstance.GetReference("user_invites").Child(myUid);

        refPath.ChildAdded += (sender, args) =>
        {
            string roomCode = args.Snapshot.Key;
            string host = args.Snapshot.Child("host").Value.ToString();

            onInvite?.Invoke(roomCode, host);
        };
    }

    
    public void RespondToInvite(string myUid, string inviteId, string response) // "accepted" or "declined"
    {
        DatabaseReference inviteRef = FirebaseDatabase.DefaultInstance
            .GetReference("invites")
            .Child(myUid)
            .Child(inviteId)
            .Child("status");

        inviteRef.SetValueAsync(response).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("초대 응답 실패");
            }
            else
            {
                Debug.Log("초대 응답 완료: " + response);
            }
        });
    }
}
