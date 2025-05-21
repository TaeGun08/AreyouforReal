using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Popup_RoomList : BaseWindow
{
    [SerializeField] private GameObject roomListParent;
    [SerializeField] private LobbyRoom roomPrefab;
    
    private readonly List<LobbyRoom> roomList = new List<LobbyRoom>();
    
    private void Awake()
    {
        roomList.AddRange(roomListParent.GetComponentsInChildren<LobbyRoom>(true)); // 비활성 포함하여 풀링
    }

    private void OnEnable()
    {
        
    }

    public void OnClickedCreateRoomButton()
    {
        //시형님이랑 작업
    }
    
    public void OnClickedJoinRoomButton()
    {
        //시형님이랑 작업
        //룸 번호를 직접 입력해서 입장
    }
    
    public async Task OnRoomInfoPanel()
    {
        //비동기로 모든 Rooms를 읽어들임
        List<RoomData> roomData = await FirestoreManager.Instance.GetAllDocumentsAsync<RoomData>(FirebaseCollections.Rooms);

        // 부족한 개수만큼 풀링 추가
        if (roomData.Count > roomList.Count)
        {
            CreateRoomInstances(roomData.Count - roomList.Count);
        }
        
        // roomData.Sort(x => x.CreatedAt.CompareTo(x.CreatedAt));
        
        int roomIndex = 0;
        
        if (roomData.Count > 0)
        {
            foreach (RoomData data in roomData)
            {
                if (data.IsGameOver.Equals(false)) //게임 완료 상태가 아닐 경우
                {
                    roomList[roomIndex].RoomSetting(data);
                    roomList[roomIndex].gameObject.SetActive(true);
                    roomIndex++;
                }
            }
        }
        
        // roomIndex가 끝난 지점부터 ~ 사용하지 않는 오브젝트들을 끕니다.
        for (int i = roomIndex; i < roomList.Count; i++)
        {
            roomList[i].gameObject.SetActive(false);
        }
    }
    
    private void CreateRoomInstances(int count)
    {
        for (int i = 0; i < count; i++)
        {
            LobbyRoom room = Instantiate(roomPrefab, roomListParent.transform);
            room.gameObject.SetActive(false);
            roomList.Add(room);
        }
    }
    
    public override void OnClickedExitButton() // roomListParent 창이 꺼질 때
    {
        foreach (var room in roomList)
        {
            room.gameObject.SetActive(false); //자식 room들을 꺼줍니다.
        }

        gameObject.SetActive(false); //후에 자신을 끕니다.
    }
}
