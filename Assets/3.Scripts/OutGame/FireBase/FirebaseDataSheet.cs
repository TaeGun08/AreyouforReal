using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

public enum FirebaseCollections
{
    Players,
    Rooms,
    MatchHistorys,
    Ranks,
}

[FirestoreData]
public class PlayerData  //key : GUID
{
    [FirestoreProperty] public string Email { get; set; }
    [FirestoreProperty] public string NickName { get; set; }
    [FirestoreProperty] public string Role { get; set; }
    [FirestoreProperty] public Timestamp CreatedAt { get; set; }
    [FirestoreProperty] public bool IsTutorialCompleted { get; set; }
    [FirestoreProperty] public List<string> Friends { get; set; } =  new List<string>();
}

[FirestoreData]
public class RoomData  //key : RoomCode
{
    [FirestoreProperty] public string RoomName { get; set; }
    [FirestoreProperty] public string RoomInfo { get; set; }
    [FirestoreProperty] public string RoomCode { get; set; }
    [FirestoreProperty] public int MembersCount { get; set; }
    [FirestoreProperty] public int MaxPlayers { get; set; }
    [FirestoreProperty] public Timestamp CreatedAt { get; set; }
    [FirestoreProperty] public bool IsGameStarted { get; set; }
    [FirestoreProperty] public bool IsGameOver { get; set; }
}

[FirestoreData]
public class MatchHistoryData  //key : Player GUID
{
    [FirestoreProperty] public string Rank { get; set; }
    [FirestoreProperty] public string KillCount { get; set; }
    [FirestoreProperty] public string PlayTime { get; set; }
}

[FirestoreData]
public class RankData  //key : Player GUID
{
    [FirestoreProperty] public string PlayerName { get; set; }
    [FirestoreProperty] public string RankPoint { get; set; }
}