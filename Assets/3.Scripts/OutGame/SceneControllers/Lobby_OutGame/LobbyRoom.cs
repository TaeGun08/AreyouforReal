using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Firestore;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LobbyRoom : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TMP_Text roomNameText;             // 제목
    [SerializeField] private TMP_Text roomInfoText;             // 설명
    [SerializeField] private TMP_Text playerCountText;          // {입장한 플레이어 / 최대 입장 가능 수}
    [SerializeField] private Button EnterButton;            // 게임 시작 버튼
    [SerializeField] private GameObject DimBackground;      // 방 입장 불가시 표시할 가림막
    [SerializeField] private GameObject joinableIcon;       // 방 입장 가능할때 표시할 아이콘
    [SerializeField] private GameObject unJoinableIcon;     // 방 입장 불가시 표시할 아이콘
    
    private string roomCode;       // 세션 코드
    private bool isGameStarted;    // 게임이 이미 시작되었는지
    private int playerCount;       // 입장한 플레이어
    private int maxPlayers;        // 최대 입장 가능 수
    
    public Timestamp createdAt { get; private set; }   //방 생성 시점 (정렬하는 용도 => 버튼으로 정렬 기능 추가용)
    
    public void RoomSetting( RoomData roomData )
    {
        roomNameText.text = roomData.RoomName;
        roomInfoText.text = roomData.RoomInfo;
        roomCode = roomData.RoomCode;
        playerCount = roomData.MembersCount;
        maxPlayers = roomData.MaxPlayers;
        
        playerCountText.text = $"{playerCount} / {maxPlayers}";
        
        createdAt = roomData.CreatedAt;
        isGameStarted = roomData.IsGameStarted;
        
        DimBackground.SetActive( isGameStarted ); // 게임이 이미 시작됐을 경우 가림막을 표시해서 보는사람에게 차이 주기
        EnterButton.interactable = !isGameStarted; // 아직 게임이 시작하지 않았을 때 클릭 활성화 : 게임이 이미 시작됐을 경우 클릭 비활성화
        
        joinableIcon.SetActive( !isGameStarted ); // 입장 가능할 때 ( 아직 게임이 시작하지 않았을 때 ) 표시되는 아이콘
        unJoinableIcon.SetActive( isGameStarted ); // 입장 불가능할 때 ( 게임이 이미 시작됐을 경우 ) 표시되는 아이콘
    }

    
    //ButtonEvent
    public void OnClickedEnterButton()
    {
        FirestoreManager.Instance.ReadDataAsync<RoomData>(FirebaseCollections.Rooms, roomCode).ContinueWithOnMainThread(
            task =>
            {
                RoomData roomData = task.Result;
                
                if (roomData.MembersCount < roomData.MaxPlayers && //최대인원수 검사
                    roomData.IsGameStarted.Equals(false) && 
                    roomData.IsGameOver.Equals(false))
                {
                    _ = NetworkStartBridge.Instance.JoinRoom(roomCode);
                }
                else
                {
                    LobbyManager.Instance.OnPopupChecking();
                }
            });
    }
}
