using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExitGames.Client.Photon.StructWrapping;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEditor;
using UnityEngine;

public class FirebaseInviteManager : MonoBehaviour
{
    public static FirebaseInviteManager Instance;
    private ListenerRegistration invitationListener;
    private FirebaseFirestore firestore;
    public bool IsInitialized { get; private set; } = false;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        DontDestroyOnLoad(gameObject);
        
        if (FirestoreManager.Instance.IsInitialized)
        {
            firestore = FirestoreManager.Instance.Firestore;
            IsInitialized = true;
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendInvitation(FirebaseMainSession.Instance.FirebaseUser.UserData.UserId, FirebaseMainSession.Instance.FirebaseUser.UserData.UserId, "ddaf");
        }
    }
    
    public void SendInvitation(string fromUid, string toUid, string roomId) //초대 메시지 전송
    {
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
        
        string docId = $"{fromUid}_{toUid}";

        _= FirestoreManager.Instance.WriteDataAsync(FirebaseCollections.Invitations, docId, invitationData);
    }
    

    /// 초대 수신 리스너
    public void ListenToInvitations(string myUid, Action<string, InvitationData> onInviteReceived)
    {
        invitationListener = firestore.Collection("Invitations")
            .WhereEqualTo("To", myUid)
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
