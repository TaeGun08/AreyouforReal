using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecordUI : MonoBehaviour
{
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text killCountText;
    [SerializeField] private TMP_Text playTimeText;

    private async void Start()
    {
        MatchHistoryData historyData = await FirestoreManager.Instance.ReadDataAsync<MatchHistoryData>(
            FirebaseCollections.MatchHistorys, "asd");
        
        rankText.SetText(historyData.Rank.ToString());
        killCountText.SetText(historyData.KillCount.ToString());
        playTimeText.SetText(historyData.PlayTime);
    }
}
