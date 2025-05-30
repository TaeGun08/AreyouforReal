using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup_JoinRoom : BaseWindow
{
    [SerializeField] private TMP_InputField roomCodeInputField;
    [SerializeField] private Button joinButton;
    
    public void OnclickJoinButton()
    {
        // _ = NetworkStartBridge.Instance.JoinRoom(roomCodeInputField.text); // 입장 가능 검사를 JoinRoom에서 하도록 수정
        
        joinButton.interactable = false;
        
        //방 입장 Client
        NetworkStartBridge.Instance.JoinRoom(roomCodeInputField.text).ContinueWithOnMainThread(
            task =>
            {
                if (task.IsFaulted || task.IsCanceled || task.Result == false)//실패
                {
                    joinButton.interactable = true; //버튼 다시 활성화
                    LobbyManager.Instance.OnPopupChecking(CheckTexts.Join);
                } 
            });
    }
}
