using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Fusion;
using UnityEngine;

public class SampleUser
{
    public FirebaseUser UserData { get; set; }
    // public PlayerRef FusionPlayerRef { get; set; }
}

public class FirebaseMainSession : MonoBehaviour
{
    public static FirebaseMainSession Instance { get; private set; }

    public SampleUser SampleUser { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        SampleUser = new SampleUser();
    }
    
    public void SetUserData(FirebaseUser user)
    {
        SampleUser.UserData = user;
        Debug.Log($"MainSystem UserId ::: {SampleUser.UserData.UserId}");
        Debug.Log($"MainSystem DisplayName ::: {SampleUser.UserData.DisplayName}");
    }
    
    // public void SetFusionPlayerRef(PlayerRef playerRef)
    // {
    //     SampleUser.FusionPlayerRef = playerRef;
    // }
}
