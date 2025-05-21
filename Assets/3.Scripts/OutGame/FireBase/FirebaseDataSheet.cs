using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;


[FirestoreData]
public class PlayerData
{
    [FirestoreProperty] public string Email { get; set; }
    [FirestoreProperty] public string NickName { get; set; }
    [FirestoreProperty] public string Role { get; set; }
    [FirestoreProperty] public Timestamp CreatedAt { get; set; }
    [FirestoreProperty] public List<string> Friends { get; set; } =  new List<string>();
}

[FirestoreData]
public class RoomData
{
    [FirestoreProperty] public string RoomName { get; set; }
    [FirestoreProperty] public string RoomCode { get; set; }
    [FirestoreProperty] public int MembersCount { get; set; }
    [FirestoreProperty] public bool IsGameStarted { get; set; }
}
//
// public class FirebaseDataSheet : MonoBehaviour
// {
//     
// }