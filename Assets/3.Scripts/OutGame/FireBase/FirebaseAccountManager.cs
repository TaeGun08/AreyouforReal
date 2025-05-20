using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;

public class FirebaseAccountManager : MonoBehaviour
{
    public static FirebaseAccountManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    private FirebaseAuth auth;
    
    private bool isInitialized = false;
    
    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies");
            }
        });
    }

    public void CreateAccount(string email, string password, string nickname)
    {
        if(isInitialized.Equals(false)) return;
        
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError(task.Exception);
                return;
            }

            var result = task.Result;
            FirebaseUser newUser = result.User;
            MainSystem.Instance.SetUserData(newUser);
            //회원가입 성공

            UpdateUserNickname(newUser, nickname); // Auth 닉네임 설정
            CreateUserDocument(newUser.UserId, email, nickname); // Firestore에도 닉네임 저장
        });
    }

    public void UpdateUserNickname(FirebaseUser user, string nickname)
    {
        if(isInitialized.Equals(false)) return;
        UserProfile profile = new UserProfile
        {
            DisplayName = nickname
        };

        user.UpdateUserProfileAsync(profile);
    }

    public void CreateUserDocument(string uid, string email, string nickname)
    {
        if(isInitialized.Equals(false)) return;
        PlayerData userData = new PlayerData()
        {
            Email = email,
            NickName = nickname,
            CreatedAt = Timestamp.GetCurrentTimestamp(),
            Role = "user"
            //Freiends
        };
        
        FirestoreManager.Instance.WriteDataAsync(FirebaseCollections.Players, uid, userData);
    }

    public bool SignIn(string email, string password)
    {
        if(isInitialized.Equals(false)) return false;
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError(task.Exception);
                return false;
            }

            var result = task.Result;
            FirebaseUser user = result.User;
            MainSystem.Instance.SetUserData(user);
            return true;
        });
        
        return false;
    }

    public void SignOut() //실행하는곳에서 login false 하기
    {
        auth.SignOut();
        MainSystem.Instance.SetUserData(null);
    }
}
