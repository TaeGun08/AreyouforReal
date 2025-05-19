using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

public enum FirebaseCollections
{
    Players,
    Rooms,
}

[FirestoreData]
public class PlayersData
{


    [FirestoreProperty] public string Name { get; set; }
    [FirestoreProperty] public string Email { get; set; }
    [FirestoreProperty] public string Role { get; set; }
    [FirestoreProperty] public List<string> Freiends { get; set; }
    
    public PlayersData(List<string> freiends)
    {
        Freiends = freiends;
    }
}

[FirestoreData]
public class RoomsData
{
    [FirestoreProperty] public string RoomName { get; set; }
    [FirestoreProperty] public string RoomCode { get; set; }
    [FirestoreProperty] public int MembersCount { get; set; }
    [FirestoreProperty] public int Freiends { get; set; }
}

public class FirebaseDataSheet : MonoBehaviour
{

}
