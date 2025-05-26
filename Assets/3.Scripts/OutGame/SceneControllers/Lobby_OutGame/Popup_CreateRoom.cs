using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fusion;
using TMPro;
using UnityEngine;

public class Popup_CreateRoom : BaseWindow
{
    [SerializeField] private TMP_InputField RoomName;   //한글 + 영어
    [SerializeField] private TMP_InputField RoomInfo;   //한글 + 영어 + 특수기호
    [SerializeField] private TMP_InputField MaxPlayers; //2~10까지 숫자만 허용
    
    private void Start()
    {
        RoomName.onValueChanged.AddListener(ValidateRoomName);
        RoomInfo.onValueChanged.AddListener(ValidateRoomInfo);
        MaxPlayers.onValueChanged.AddListener(ValidateMaxPlayers);
    }

    private void ValidateRoomName(string input)
    {
        // 한글(가-힣) + 영어(대소문자)만 허용
        string result = Regex.Replace(input, @"[^a-zA-Z가-힣]", "");
        if (RoomName.text != result)
            RoomName.text = result;
    }

    private void ValidateRoomInfo(string input)
    {
        // 한글(가-힣) + 영어(대소문자) + 특수기호 허용 (공백 포함)
        // 특수기호는 정해진 범위를 사용할 수 있음. 예: 기본 특수기호만 허용
        string result = Regex.Replace(input, @"[^a-zA-Z가-힣\s~`!@#$%^&*()\-_=+\[\]{};:'"",.<>/?\\|]", "");
        if (RoomInfo.text != result)
            RoomInfo.text = result;
    }

    private void ValidateMaxPlayers(string input)
    {
        // 숫자만 허용하고, 2~10 사이 숫자만 허용
        if (!int.TryParse(input, out int number) || number < 2 || number > 10)
        {
            MaxPlayers.text = ""; // 잘못된 값이면 비움 또는 이전 값 저장 방식 적용 가능
        }
    }
    
    public void OnClickedCreateButton()
    {
        if (RoomName.text.Equals("")  || RoomInfo.text.Equals("")  || MaxPlayers.text.Equals("") ) //예외처리, 경고문 표시
        {
            LobbyManager.Instance.OnPopupChecking(CheckTexts.Create);
            return;
        }
        
        //방 입장 Host
        _ = NetworkStartBridge.Instance.CreateRoom(RoomName.text, RoomInfo.text, MaxPlayers.text);
    }
}
