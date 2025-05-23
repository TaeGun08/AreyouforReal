using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Fusion;
using UnityEngine;

public class FirebaseUser
{
    public Firebase.Auth.FirebaseUser UserData { get; set; }
    // public PlayerRef FusionPlayerRef { get; set; }
}

public class FirebaseMainSession : MonoBehaviour
{
    public static FirebaseMainSession Instance { get; private set; }

    public FirebaseUser FirebaseUser { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        FirebaseUser = new FirebaseUser();
    }
    
    public void SetUserData(Firebase.Auth.FirebaseUser user)
    {
        FirebaseUser.UserData = user;

        if (user != null) //디버그용
        {
            Debug.Log($"MainSystem UserId ::: {FirebaseUser.UserData.UserId}");
            Debug.Log($"MainSystem DisplayName ::: {FirebaseUser.UserData.DisplayName}");
        }
    }
    
    // public void SetFusionPlayerRef(PlayerRef playerRef)
    // {
    //     SampleUser.FusionPlayerRef = playerRef;
    // }
}
