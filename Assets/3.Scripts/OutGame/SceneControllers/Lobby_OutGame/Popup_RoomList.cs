using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseRoomManager : BaseWindow
{
    //필요한것 : 리스트 생성
    [SerializeField] private GameObject roomList;  //리스트 자식으로
    
    // public class LobbyRoom
    // {
    //     //인원수
    //     //게임이 시작했는지
    //     
    // }

    private List<LobbyRoom> rooms;

    private void Start()
    {
        OnRoomInfoPanel();
    }

    public void MakeRoom()
    {
        //필요한것 : 룸 식별자 (키), 만 가지고 있고 나머지는 키를 통해 불러오자  //룸 코드(세션코드), 
        
    }

    public async Task OnRoomInfoPanel()
    {
        List<RoomData> roomData = await FirestoreManager.Instance.GetAllDocumentsAsync<RoomData>(FirebaseCollections.Rooms);

        if (roomData.Count > 0)
        {
            
        }
    }
}
