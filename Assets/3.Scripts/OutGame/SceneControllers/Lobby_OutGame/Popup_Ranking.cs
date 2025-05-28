using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Extensions;
using Fusion;
using UnityEngine;

public class Popup_Ranking : BaseWindow
{
    [Header("Rank")]
    [SerializeField] private GameObject rankListParent;
    [SerializeField] private Rank myRank;
    [SerializeField] private Rank rankPrefab;
    
    private List<Rank> rankList;
    
    private void Awake()
    {
        rankList = new List<Rank>();
        rankList.AddRange(rankListParent.GetComponentsInChildren<Rank>(true)); // 비활성 포함하여 풀링
    }
    
    private void OnEnable()
    {
        _ = OnRankInfoPanel();
    }
    
    private async Task OnRankInfoPanel()
    {
        //비동기로 모든 Rank를 읽어들임
        Dictionary<string,RankData> rankDataDict  = await FirestoreManager.Instance.GetAllDocumentsWithKeyAsync<RankData>(FirebaseCollections.Ranks);

        // 부족한 개수만큼 풀링 추가
        if (rankDataDict.Count > rankList.Count)
        {
            CreateRankInstances(rankDataDict.Count - rankList.Count);
        }
        
        string myUserId = FirebaseMainSession.Instance.FirebaseUser.UserData.UserId;
        
        // RankPoint기준 내림차순으로 정렬
        var sortedRankList = rankDataDict
            .OrderByDescending(pair => pair.Value.RankPoint)
            .ToList();
        
        int myRankIndex = sortedRankList.FindIndex(pair => pair.Key == myUserId);
        myRank.RankSetting((myRankIndex + 1).ToString(), sortedRankList[myRankIndex].Value);
        
        int rankIndex = 0;
        
        if (sortedRankList.Count > 0)
        {
            foreach (var rank in sortedRankList)
            {
                rankList[rankIndex].RankSetting((rankIndex + 1).ToString(),rank.Value);
                rankList[rankIndex].gameObject.SetActive(true);
                rankIndex++;
            }
        }
        
        // roomIndex가 끝난 지점부터 ~ 사용하지 않는 오브젝트들을 끕니다.
        for (int i = rankIndex; i < rankList.Count; i++)
        {
            rankList[i].gameObject.SetActive(false);
        }
    }
    
    private void CreateRankInstances(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Rank rank = Instantiate(rankPrefab, rankListParent.transform);
            rank.gameObject.SetActive(false);
            rankList.Add(rank);
        }
    }
}
