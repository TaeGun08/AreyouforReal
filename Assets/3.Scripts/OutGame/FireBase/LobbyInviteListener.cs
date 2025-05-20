// using Firebase.Database;
// using UnityEngine;
//
// public class LobbyInviteListener : MonoBehaviour
// {
//     private DatabaseReference lobbyRef;
//
//     void Start()
//     {
//         // Firebase 경로 설정 (예: "Lobby/Invitations")
//         lobbyRef = FirebaseDatabase.DefaultInstance.GetReference("Lobby").Child("Invitations");
//
//         // 실시간 리스너 등록
//         lobbyRef.ChildAdded += OnInvitationAdded;
//         lobbyRef.ChildChanged += OnInvitationChanged;
//         lobbyRef.ChildRemoved += OnInvitationRemoved;
//     }
//
//     // 초대가 추가될 때 호출
//     void OnInvitationAdded(object sender, ChildChangedEventArgs args)
//     {
//         if (args.Snapshot.Exists)
//         {
//             Debug.Log("새로운 초대 요청이 도착했습니다: " + args.Snapshot.GetRawJsonValue());
//         }
//     }
//
//     // 초대 내용이 변경될 때 호출
//     void OnInvitationChanged(object sender, ChildChangedEventArgs args)
//     {
//         Debug.Log("초대 상태가 변경되었습니다: " + args.Snapshot.GetRawJsonValue());
//     }
//
//     // 초대가 삭제될 때 호출
//     void OnInvitationRemoved(object sender, ChildChangedEventArgs args)
//     {
//         Debug.Log("초대가 취소되었습니다: " + args.Snapshot.Key);
//     }
// }