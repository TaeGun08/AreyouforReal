using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_Setting : BaseWindow
{
    public void OnClickedUserIdButton()
    {
        GUIUtility.systemCopyBuffer = FirebaseMainSession.Instance.FirebaseUser.UserData.UserId;
        LobbyManager.Instance.OnPopupChecking(CheckTexts.CopyMyId);
    }
}
