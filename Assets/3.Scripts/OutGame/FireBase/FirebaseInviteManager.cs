using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEditor;
using UnityEngine;

public class FirebaseInviteManager : MonoBehaviour
{
    public static FirebaseInviteManager Instance;
    private ListenerRegistration invitationListener;
    
    private void Awake()
    {
        Instance = this;
    }

    public async Task SendInvitation(string fromUid, string toUid, string roomId)
    {
        Debug.Log("초대 메시지 전송 완료");
        
        InvitationData invitationData = new InvitationData
        {
            From = fromUid,
            To = toUid,
            Type = "game_invite",
            RoomId = roomId, // 친구 요청이라면 비워진 상태
            Message = "",    // 친구 요청이라면 메시지 사용
            Timestamp = Timestamp.GetCurrentTimestamp(),
            Status = InvitationStatus.Pending.ToString()
        };

        string docId = Guid.NewGuid().ToString();

        await FirestoreManager.Instance.WriteDataAsync(FirebaseCollections.Invitations, docId, invitationData)
            .ContinueWithOnMainThread(
                task =>
                {
                    if (task.IsFaulted || task.IsCanceled)
                        Debug.LogError("초대 전송 실패: " + task.Exception);
                    else
                        Debug.Log("초대 전송 완료");
                });
    }
    

    /// 초대 수신 리스너
    public void ListenToInvitations(string myUid, Action<string, InvitationData> onInviteReceived)
    {
        invitationListener = FirestoreManager.Instance.Firestore
            .Collection(FirebaseCollections.Invitations.ToString())
            .WhereEqualTo("to", myUid)
            .WhereEqualTo("status", InvitationStatus.Pending.ToString())
            .Listen(snapshot =>
            {
                foreach (var doc in snapshot.Documents)
                {
                    InvitationData invite = doc.ConvertTo<InvitationData>();
                    onInviteReceived?.Invoke(doc.Id, invite);
                }
            });
    }

    /// 리스너 정리
    public void StopListening()
    {
        invitationListener?.Stop();
        invitationListener = null;
    }

    /// 초대 응답 (수락 또는 거절)
    public async Task RespondToInvitation(string docId, InvitationStatus newStatus)
    {
        var updateDict = new Dictionary<string, object>
        {
            { "Status", newStatus.ToString() }
        };

        await FirestoreManager.Instance.UpdateDataAsync(
            FirebaseCollections.Invitations,
            docId,
            updateDict
        );
    }
}
