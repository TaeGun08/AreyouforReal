using System.Collections;
using System.Collections.Generic;
using Firebase;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    void Start()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        
        DocumentReference docRef = db.Collection("Test").Document("Test");
        Dictionary<string, object> city = new Dictionary<string, object>
        {
            { "Test", "Los Angeles" },
            { "State", "CA" },
            { "Country", "USA" }
        };
        docRef.SetAsync(city).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the Test document in the cities collection.");
        });
    }
}
