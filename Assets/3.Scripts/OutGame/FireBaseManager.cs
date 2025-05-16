using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class FireBaseManager : MonoBehaviour
{
    void Start()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        
        DocumentReference docRef = db.Collection("cities").Document("LA");
        Dictionary<string, object> city = new Dictionary<string, object>
        {
            { "Name", "Los Angeles" },
            { "State", "CA" },
            { "Country", "USA" }
        };
        docRef.SetAsync(city).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the LA document in the cities collection.");
        });
    }
}
