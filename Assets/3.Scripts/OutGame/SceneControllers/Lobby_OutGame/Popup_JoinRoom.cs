using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class Popup_JoinRoom : BaseWindow
{
    [SerializeField] private TMP_InputField roomCodeInputField;
    
    public async Task OnclickJoinButton()
    {
        //비동기로 모든 Rooms를 읽어들임
        List<RoomData> roomData = await FirestoreManager.Instance.GetAllDocumentsAsync<RoomData>(FirebaseCollections.Rooms);

        foreach (RoomData data in roomData)
        {
            
        }

        FirestoreManager.Instance.ReadDataAsync<RoomData>(FirebaseCollections.Rooms, roomCodeInputField.text).ContinueWithOnMainThread(
            task =>
            {
                RoomData roomData = task.Result;
                
                if (roomData.MembersCount < roomData.MaxPlayers && //최대인원수 검사
                    roomData.IsGameStarted.Equals(false) && 
                    roomData.IsGameOver.Equals(false))
                {
                    _ = NetworkStartBridge.Instance.JoinRoom(roomCodeInputField.text);
                }
                else
                {
                    LobbyManager.Instance.OnPopupChecking();
                }
            });
    }
}
