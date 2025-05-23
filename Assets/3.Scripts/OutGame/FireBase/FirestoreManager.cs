using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;

public enum FirebaseCollections
{
    Players,
    Rooms,
}

public class FirestoreManager : MonoBehaviour
{
    private FirebaseFirestore firestore;
    private DocumentReference docRef;
    private bool isInitialized = false;

    public static FirestoreManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        DontDestroyOnLoad(gameObject);
        
        if (FirebaseAuth.DefaultInstance.CurrentUser == null) //디버그용 자동 로그인 Admin 확인 필요
        {
            Debug.LogWarning("Firebase User is not authenticated!");
            FirebaseAccountManager.Instance.SignIn("admin@gmail.com", "admin123");
        }
    }

    private void Start()
    {
        InitializeFirebase();
    }

    // Firebase 초기화
    public void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                firestore = FirebaseFirestore.DefaultInstance;
                isInitialized = true;
                Debug.Log("Firebase Firestore Initialized Successfully");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
            }
        });
    }

    // 데이터 쓰기 (Collection과 Key 기반)
    public async Task<bool> WriteDataAsync<T>(FirebaseCollections collection, string key, T data)
    {

        if (isInitialized.Equals(false))
        {
            Debug.LogError("Firebase is not initialized.");
            return false;
        }
        
        Debug.Log(collection);
        Debug.Log(key);
        Debug.Log(data);

        // return false;
        
        try
        {
            docRef = firestore.Collection(collection.ToString()).Document(key);
            await docRef.SetAsync(data);
            Debug.Log($"Data written to {collection}/{key}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write data: {e.Message}");
            return false;
        }
    }

    // 데이터 쓰기 (Collection과 Key 기반)
    public void WriteDataAsync_Test<T>(FirebaseCollections collection, string key, T data)
    {
        if (isInitialized.Equals(false))
        {
            Debug.LogError("Firebase is not initialized.");
            return;
        }

        try
        {
            DocumentReference docRef = firestore.Collection(collection.ToString()).Document(key);
            docRef.SetAsync(data);
            Debug.Log($"Data written to {collection}/{key}");
            return;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write data: {e.Message}");
            return;
        }
    }
    
    // 데이터 읽기 (Collection과 Key 기반)
    public async Task<T> ReadDataAsync<T>(FirebaseCollections collection, string key) where T : class
    {
        if (isInitialized.Equals(false))
        {
            Debug.LogError("Firebase is not initialized.");
            return null;
        }

        try
        {
            DocumentReference docRef = firestore.Collection(collection.ToString()).Document(key);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<T>();
            }
            else
            {
                Debug.Log($"No data found at {collection}/{key}");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to read data: {e.Message}");
            return null;
        }
    }

    // 데이터 업데이트 (Collection과 Key 기반)
    #region 사용설명서
    // public async Task Sample()
    // {
    //     var sampleDic = new Dictionary<string, object> //전달할 딕셔너리
    //     {
    //          변경할 위치        변경할 값
    //         {"SampleField",  "SampleValue"} // 필드, 값 형태로 전달시 일치하는 필드의 값을 변경
    //     };
    //         
    //     await UpdateDataAsync(FirebaseCollections.Rooms, "Key", sampleDic); //실행
    // }
    #endregion
    public async Task UpdateDataAsync(FirebaseCollections collection, string key, Dictionary<string, object> updates)
    {
        if (isInitialized.Equals(false))
        {
            Debug.LogError("Firebase is not initialized.");
            return;
        }
        
        try
        {
            DocumentReference docRef = firestore.Collection(collection.ToString()).Document(key);
            await docRef.UpdateAsync(updates);
            Debug.Log($"Data updated at {collection}/{key}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to update data: {e.Message}");
        }
    }

    // 데이터 삭제 (Collection과 Key 기반)
    public async Task DeleteDataAsync(FirebaseCollections collection, string key)
    {
        if (isInitialized.Equals(false))
        {
            Debug.LogError("Firebase is not initialized.");
            return;
        }

        try
        {
            DocumentReference docRef = firestore.Collection(collection.ToString()).Document(key);
            await docRef.DeleteAsync();
            Debug.Log($"Data deleted from {collection}/{key}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to delete data: {e.Message}");
        }
    }
    
    //콜렉션 내의 모든 문서 읽어오기
    public async Task<List<T>> GetAllDocumentsAsync<T>(FirebaseCollections collection) where T : class
    {
        if (isInitialized.Equals(false))
        {
            Debug.LogError("Firebase is not initialized.");
            return null;
        }

        try
        {
            CollectionReference colRef = firestore.Collection(collection.ToString());
            QuerySnapshot snapshot = await colRef.GetSnapshotAsync();

            List<T> allDocs = new List<T>();
            foreach (DocumentSnapshot doc in snapshot.Documents)
            {
                if (doc.Exists)
                {
                    allDocs.Add(doc.ConvertTo<T>());
                }
            }

            Debug.Log($"Fetched {allDocs.Count} documents from {collection}");
            return allDocs;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to read all documents: {e.Message}");
            return null;
        }
    }
}
