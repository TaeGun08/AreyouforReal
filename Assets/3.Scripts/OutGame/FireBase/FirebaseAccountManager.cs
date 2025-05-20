using System.Collections.Generic;
using System.Threading.Tasks;
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
        Instance = this;
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
                isInitialized = true;
                Debug.Log("Firebase Auth Initialized Successfully");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies");
            }
        });
    }

    private TaskCompletionSource<bool> resultTcs;
    
    public Task<bool> CreateAccount(string email, string password, string nickname)
    {
        if (isInitialized.Equals(false))
        {
            Debug.LogError("Firebase is not initialized.");
            return null;
        }
        
        resultTcs = new TaskCompletionSource<bool>();
        
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
        
        return resultTcs.Task;
    }

    private void UpdateUserNickname(FirebaseUser user, string nickname)
    {
        if (isInitialized.Equals(false))
        {
            Debug.LogError("Firebase is not initialized.");
            return;
        }
        
        UserProfile profile = new UserProfile
        {
            DisplayName = nickname
        };

        user.UpdateUserProfileAsync(profile);
    }

    private void CreateUserDocument(string uid, string email, string nickname)
    {
        if (isInitialized.Equals(false))
        {
            Debug.LogError("Firebase is not initialized.");
            return;
        }
        
        PlayerData userData = new PlayerData()
        {
            Email = email,
            NickName = nickname,
            CreatedAt = Timestamp.GetCurrentTimestamp(),
            Role = "user"
            //Freiends
        };
        
        FirestoreManager.Instance.WriteDataAsync<PlayerData>(FirebaseCollections.Players, uid, userData).ContinueWithOnMainThread(
            task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    Debug.LogError(task.Exception);
                    return;
                }
                
                resultTcs.TrySetResult(true);
            });
    }

    public async Task<bool> SignIn(string email, string password)
    {
        bool isSignIn = false;
        
        if (isInitialized.Equals(false))
        {
            Debug.LogError("Firebase is not initialized.");
            return isSignIn;
        }
        
        
        await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError(task.Exception);
                return;
            }

            isSignIn = true;
            var result = task.Result;
            FirebaseUser user = result.User;
            MainSystem.Instance.SetUserData(user);
        });

        return isSignIn;
    }

    public void SignOut() //실행하는곳에서 login false 하기
    {
        auth.SignOut();
        MainSystem.Instance.SetUserData(null);
    }
}
