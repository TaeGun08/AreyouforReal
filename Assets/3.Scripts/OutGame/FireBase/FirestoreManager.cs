using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;

public class FirestoreManager : MonoBehaviour
{
    private FirebaseFirestore firestore;
    private bool isInitialized = false;
    
    public static FirestoreManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
    public async Task WriteDataAsync<T>(FirebaseCollections collection, string key, T data)
    {
        if (!isInitialized)
        {
            Debug.LogError("Firebase is not initialized.");
            return;
        }

        try
        {
            DocumentReference docRef = firestore.Collection(collection.ToString()).Document(key);
            await docRef.SetAsync(data);
            Debug.Log($"Data written to {collection}/{key}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write data: {e.Message}");
        }
    }

    // 데이터 읽기 (Collection과 Key 기반)
    public async Task<T> ReadDataAsync<T>(FirebaseCollections collection, string key) where T : class
    {
        if (!isInitialized)
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
    public async Task UpdateDataAsync(FirebaseCollections collection, string key, Dictionary<string, object> updates)
    {
        if (!isInitialized)
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
        if (!isInitialized)
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
}
