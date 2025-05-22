using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyRoom : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TMP_Text roomName;             // 제목
    [SerializeField] private TMP_Text roomInfo;             // 설명
    [SerializeField] private Button EnterButton;            // 게임 시작 버튼
    [SerializeField] private GameObject DimBackground;      // 방 입장 불가시 표시할 가림막
    [SerializeField] private GameObject joinableIcon;       // 방 입장 가능할때 표시할 아이콘
    [SerializeField] private GameObject unJoinableIcon;     // 방 입장 불가시 표시할 아이콘
    
    private string roomCode;       //세션 코드
    private bool isGameOver;       //게임 종료 여부
    public Timestamp createdAt { get; private set; }   //방 생성 시점 (정렬하는 용도 => 버튼으로 정렬 기능 추가용)
    
    public void RoomSetting( RoomData roomData )
    {
        roomName.text = roomData.RoomName;
        roomInfo.text = roomData.RoomInfo;
        roomCode = roomData.RoomCode;
        createdAt = roomData.CreatedAt;
        
        bool isGameStarted = roomData.IsGameStarted;
        
        DimBackground.SetActive( isGameStarted ); // 게임이 이미 시작됐을 경우 가림막을 표시해서 보는사람에게 차이 주기
        EnterButton.interactable = !isGameStarted; // 아직 게임이 시작하지 않았을 때 클릭 활성화 : 게임이 이미 시작됐을 경우 클릭 비활성화
        
        joinableIcon.SetActive( !isGameStarted ); // 입장 가능할 때 ( 아직 게임이 시작하지 않았을 때 )
        unJoinableIcon.SetActive( isGameStarted ); // 입장 불가능할 때 ( 게임이 이미 시작됐을 경우 )
    }

    //ButtonEvent
    public void OnClickedEnterButton()
    {
         // NetworkStartBridge_OutGameCopy.Instance.SetCode(roomCode);
         // _ = NetworkStartBridge_OutGameCopy.Instance.StartGame(GameMode.Client);
    }
}
