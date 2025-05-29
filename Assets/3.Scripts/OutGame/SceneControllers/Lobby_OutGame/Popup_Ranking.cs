using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Extensions;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;

public class Popup_Ranking : BaseWindow
{
    [Header("Match History")]
    [SerializeField] private GameObject matchListParent;
    [SerializeField] private MatchHistory matchPrefab;
    
    private List<MatchHistory> matchList;
    
    private void Awake()
    {
        matchList = new List<MatchHistory>();
        matchList.AddRange(matchListParent.GetComponentsInChildren<MatchHistory>(true)); // 비활성 포함하여 풀링
    }
    
    private void OnEnable()
    {
        _ = OnRankInfoPanel();
    }
    
    private async Task OnRankInfoPanel()
    {
        List<MatchHistoryData> matchHistoryDatas =
            await FirestoreManager.Instance.GetAllDocumentsAsync<MatchHistoryData>(FirebaseCollections.MatchHistorys);
        
        string myUserId = FirebaseMainSession.Instance.FirebaseUser.UserData.UserId;
        
        List<MatchHistoryData> filteredList = matchHistoryDatas
            .Where(data => data.PlayerKey == myUserId)
            .ToList();
        
        // 부족한 개수만큼 풀링 추가
        if (filteredList.Count > matchList.Count)
        {
            CreateRankInstances(filteredList.Count - matchList.Count);
        }
        
        int matchIndex = 0;
        
        if (filteredList.Count > 0)
        {
            foreach (var match in filteredList)
            {
                matchList[matchIndex].MatchHistorySetting(match);
                matchList[matchIndex].gameObject.SetActive(true);
                matchIndex++;
            }
        }
        
        // roomIndex가 끝난 지점부터 ~ 사용하지 않는 오브젝트들을 끕니다.
        for (int i = matchIndex; i < matchList.Count; i++)
        {
            matchList[i].gameObject.SetActive(false);
        }
    }
    
    private void CreateRankInstances(int count)
    {
        for (int i = 0; i < count; i++)
        {
            MatchHistory rank = Instantiate(matchPrefab, matchListParent.transform);
            rank.gameObject.SetActive(false);
            matchList.Add(rank);
        }
    }
}
