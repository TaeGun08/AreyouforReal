using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class Popup_JoinRoom : BaseWindow
{
    [SerializeField] private TMP_InputField roomCodeInputField;
    
    public void OnclickJoinButton()
    {
        _ = NetworkStartBridge.Instance.JoinRoom(roomCodeInputField.text); // 입장 가능 검사를 JoinRoom에서 하도록 수정
    }
}
