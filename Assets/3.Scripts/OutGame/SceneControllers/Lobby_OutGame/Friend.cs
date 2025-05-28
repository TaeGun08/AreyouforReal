using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Friend : MonoBehaviour
{
    [SerializeField] private TMP_Text FriendNameText;
    public string FriendName { get; private set; }
    public string FriendKey { get; private set; }
    
    
    public void SetFriend(string nameText,  string keyText)
    {
        FriendNameText.text = nameText;
        FriendName = nameText;
        FriendKey = keyText;
    }
}
