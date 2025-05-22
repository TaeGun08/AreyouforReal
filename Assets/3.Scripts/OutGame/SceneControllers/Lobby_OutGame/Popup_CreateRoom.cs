using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using TMPro;
using UnityEngine;

public class Popup_CreateRoom : BaseWindow
{
    [SerializeField] private TMP_InputField RoomName;
    [SerializeField] private TMP_InputField RoomInfo;
    [SerializeField] private TMP_InputField MembersCount; //일단은 직접 입력 2~10까지

    
    public void OnClickedCreateButtonWrapper()
    {
        _ = OnClickedCreateButton();
    }
    
    private async Task OnClickedCreateButton()
    {
        //방 입장 Host
        NetworkStartBridge_OutGameCopy.Instance.StartGame(GameMode.Host, RoomName.text, RoomInfo.text, MembersCount.text);
    }
}
