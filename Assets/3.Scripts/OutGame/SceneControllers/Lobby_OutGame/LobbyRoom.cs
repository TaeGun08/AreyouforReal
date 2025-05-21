using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyRoom : MonoBehaviour
{
    //방 제목, 설명, 코드, 게임 시작 여부 표시용(실제로 확인해 봐야 함(갱신 이슈))
    [SerializeField] private TMP_Text roomNameText;
    [SerializeField] private TMP_Text roomInfoText;
    
    // [SerializeField] private bool createdAt;
    [SerializeField] private bool isGameStarted;

    [HideInInspector] public string roomCode;
}
