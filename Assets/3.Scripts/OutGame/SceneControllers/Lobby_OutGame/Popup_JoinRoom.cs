using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Popup_JoinRoom : BaseWindow
{
    [SerializeField] private TMP_InputField roomCodeInputField;

    
    
    public void OnclickJoinButton()
    {
        // List<RoomData> roomData = await FirestoreManager.Instance.GetAllDocumentsAsync<RoomData>(FirebaseCollections.Rooms);
        // foreach (RoomData room in roomData)
        // {
        //     room.MembersCount
        //         room.MaxPlayers
        // }
        
        NetworkStartBridge.Instance.JoinRoom(roomCodeInputField.text);
    }
}
