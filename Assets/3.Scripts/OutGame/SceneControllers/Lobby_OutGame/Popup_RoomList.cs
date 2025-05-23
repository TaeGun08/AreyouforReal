using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Popup_RoomList : BaseWindow
{
    [Header("Room")]
    [SerializeField] private GameObject roomListParent;
    [SerializeField] private LobbyRoom roomPrefab;
    
    [Header("PopUp")]
    [SerializeField] private Popup_CreateRoom popupCreateRoom;
    [SerializeField] private Popup_JoinRoom popupJoinRoom;
    
    private List<LobbyRoom> roomList;
    
    private void Awake()
    {
        roomList = new List<LobbyRoom>();
        roomList.AddRange(roomListParent.GetComponentsInChildren<LobbyRoom>(true)); // 비활성 포함하여 풀링
    }

    private void OnEnable()
    {
        _ = OnRoomInfoPanel();
    }
    
    public void OnClickedCreateRoomButton()
    {
        //방 만드는 창을 띄워줍니다
        popupCreateRoom.gameObject.SetActive(true);
    }
    
    public void OnClickedJoinRoomButton()
    {
        //시형님이랑 작업
        //룸 번호를 직접 입력해서 입장
        popupJoinRoom.gameObject.SetActive(true);
    }
    
    public void OnClickedResetButton()
    {
        _ = OnRoomInfoPanel();
    }
    
    public override void OnClickedExitButton() // roomListParent 창이 꺼질 때
    {
        foreach (var room in roomList)
        {
            room.gameObject.SetActive(false); //자식 room들을 꺼줍니다.
        }

        gameObject.SetActive(false); //후에 자신을 끕니다.
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
        
        //데이터를 방 생성 시점 기준 (CreatedAt) 최신순으로 정렬
        roomData.Sort((a, b) => b.CreatedAt.ToDateTime().CompareTo(a.CreatedAt.ToDateTime())); //내림차순 정렬
        //roomData.Sort((a, b) => a.CreatedAt.ToDateTime().CompareTo(b.CreatedAt.ToDateTime())); //오름차순 정렬
        
        int roomIndex = 0;
        
        if (roomData.Count > 0)
        {
            foreach (var data in roomData.Where(data => data.IsGameOver.Equals(false)))
            {
                roomList[roomIndex].RoomSetting(data);
                roomList[roomIndex].gameObject.SetActive(true);
                roomIndex++;
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
}
