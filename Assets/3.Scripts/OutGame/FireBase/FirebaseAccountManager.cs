// using UnityEngine;
// using Firebase;
// using Firebase.Auth;
// using Firebase.Firestore;
// using Firebase.Extensions;
//
// public class FirebaseAccountManager : MonoBehaviour
// {
//     private FirebaseAuth auth;
//     // private FirebaseFirestore firestore;
//     
//     private bool isInitialized = false;
//     private bool isLoggedIn = false;
//     private bool isSignUpMode = false;
//
//     private void Start()
//     {
//         FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
//         {
//             if (task.Result == DependencyStatus.Available)
//             {
//                 auth = FirebaseAuth.DefaultInstance;
//                 isInitialized = true;
//             }
//             else
//             {
//                 Debug.LogError("Could not resolve all Firebase dependencies");
//             }
//         });
//     }
//
//     private void CreateAccount(string email, string password, string nickname)
//     {
//         auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
//         {
//             if (task.IsCanceled || task.IsFaulted)
//             {
//                 Debug.LogError(task.Exception);
//                 return;
//             }
//
//             var result = task.Result;
//             FirebaseUser newUser = result.User;
//             MainSystem.Instance.SetUserData(newUser);
//             //회원가입 성공
//
//             UpdateUserNickname(newUser, nickname); // Auth 닉네임 설정
//             CreateUserDocument(newUser.UserId, email, nickname); // Firestore에도 닉네임 저장
//             isSignUpMode = false;
//             isLoggedIn = true;
//         });
//     }
//
//     private void UpdateUserNickname(FirebaseUser user, string nickname)
//     {
//         UserProfile profile = new UserProfile
//         {
//             DisplayName = nickname
//         };
//
//         user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task =>
//         {
//             if (task.IsCompletedSuccessfully)
//                 statusMessage += $"\n닉네임 설정 완료: {nickname}";
//             else
//                 statusMessage += "\n닉네임 설정 실패: " + task.Exception?.Message;
//         });
//     }
//
//     private void CreateUserDocument(string uid, string email, string nickname)
//     {
//         var userDoc = firestore.Collection("users").Document(uid);
//         var userData = new
//         {
//             email = email,
//             nickname = nickname,
//             createdAt = Timestamp.GetCurrentTimestamp(),
//             role = "user"
//         };
//
//         userDoc.SetAsync(userData).ContinueWithOnMainThread(task =>
//         {
//             if (task.IsCompletedSuccessfully)
//                 statusMessage += "\nFirestore 사용자 문서 생성 완료";
//             else
//                 statusMessage += "\nFirestore 문서 생성 실패: " + task.Exception?.Message;
//         });
//     }
//
//     private void SignIn(string email, string password)
//     {
//         statusMessage = "로그인 시도 중...";
//         auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
//         {
//             if (task.IsCanceled || task.IsFaulted)
//             {
//                 statusMessage = "로그인 실패: " + task.Exception?.Message;
//                 return;
//             }
//
//             var result = task.Result;
//             FirebaseUser user = result.User;
//             MainSystem.Instance.SetUserData(user);
//             isLoggedIn = true;
//             statusMessage = $"로그인 성공: {user.Email}";
//         });
//     }
//
//     private void SignOut()
//     {
//         auth.SignOut();
//         isLoggedIn = false;
//         statusMessage = "로그아웃 되었습니다.";
//         MainSystem.Instance.SetUserData(null);
//     }
// }
