using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchHistory : MonoBehaviour
{
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text killCountText;
    [SerializeField] private TMP_Text playTimeText;
    
    public void MatchHistorySetting(MatchHistoryData matchHistoryData)
    {
        rankText.text = $"{matchHistoryData.Rank} 위";
        killCountText.text = matchHistoryData.KillCount.ToString();
        playTimeText.text = matchHistoryData.PlayTime;
    }
}
