using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Friend_Invite : Friend
{
    //초대 버튼이 있는 버전의 Friend 프리펩
    public void OnClickedInviteButton() //초대 버튼 클릭
    {
        Popup_Confirm.PopUpUI.OpenPopUI(SendInvitation_Action);
    }

    private void SendInvitation_Action()
    {
        FirebaseInviteManager.Instance.SendInvitation(
            FirebaseMainSession.Instance.FirebaseUser.UserData.UserId, //from
            FriendKey,   //to (Friend)
            InGameUIManager_OutGame.Instance.GetRoomCode() //roomCode
        );
    }
}
