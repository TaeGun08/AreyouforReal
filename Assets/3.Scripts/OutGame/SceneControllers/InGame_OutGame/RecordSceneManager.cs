using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Firestore;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordSceneManager : MonoBehaviour
{
    [SerializeField] private TMP_Text userNameText;
    [SerializeField] private TMP_Text recordRankingText;
    [SerializeField] private TMP_Text getRankingPointText;
    [SerializeField] private TMP_Text userRankingPointText;
    
    private bool isEndRecording = false;
    public static RecordSceneManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("SaveHistoryData")))
        {
            isEndRecording = true;
            return;
        }
        
        MatchHistoryData data = JsonConvert.DeserializeObject<MatchHistoryData>(PlayerPrefs.GetString("SaveHistoryData"));
        _ = RecordSceneSetup(data);
    }
    
    public async Task RecordSceneSetup(MatchHistoryData matchHistoryData)
    {
        FirebaseUser user = FirebaseMainSession.Instance.FirebaseUser;
        
        int currentRankPoint = 0;
        
        userNameText.text = user.Username; //플레이어 이름
        recordRankingText.text = matchHistoryData.Rank.ToString(); //랭크 매겨주기
        
        int getRankingPoint = CalculateRankingPoint(matchHistoryData.Rank, matchHistoryData.Players.Count); //랭킹 포인트 환산
        
        getRankingPointText.text = getRankingPoint.ToString();

        //유저의 현재 랭크포인트를 읽어옴. 데이터 없었을 경우 예외처리
        FirestoreManager.Instance.ReadDataAsync<PlayerData>(FirebaseCollections.Players, user.UserData.UserId).ContinueWithOnMainThread(
            task =>
            {
                PlayerData playerData = task.Result;
                if (task.IsFaulted || task.IsCanceled)
                {
                    // currentRankPoint = 0;
                    return;
                }
                currentRankPoint = playerData.RankPoint;
            });
        
        //userRankingPointText.text = currentRankPoint.ToString();
        int SumRankingPoint = currentRankPoint + getRankingPoint;
        userRankingPointText.text = SumRankingPoint.ToString();

        Dictionary<string, object> rankingPointUpdate = new Dictionary<string, object>
        {
            { "RankPoint", FieldValue.Increment(SumRankingPoint) }
        };
        
        await FirestoreManager.Instance.UpdateDataAsync(FirebaseCollections.Players, user.UserData.UserId, rankingPointUpdate);
        
        string uuid = Guid.NewGuid().ToString();
        
        //업데이트 없이 덮어쓰기 => MatchHistorys 없는 경우 오류 방지하고 새로쓰기
        await FirestoreManager.Instance.WriteDataAsync(FirebaseCollections.MatchHistorys, uuid, matchHistoryData)
            .ContinueWithOnMainThread(task =>
            {
                if(task.IsFaulted || task.IsCanceled) return;
                
                isEndRecording =  true;
            });
    }

    public void OnClickHomeButton()
    {
        if(!isEndRecording) return; // RecordSceneSetup 기록이 완료되지 않았음
        LoadingSceneManager.LoadScene("Lobby");
    }
    
    public int CalculateRankingPoint(int rank, int totalPlayers)
    {
        if (totalPlayers <= 1) return 0; // 에러 방지 또는 단독 플레이는 점수 없음
        if (rank < 1 || rank > totalPlayers) return 0; // 유효 범위 체크

        int maxPoint = 100;
        int minPoint = 10;

        // 등수별 포인트 = maxPoint에서 선형 감소
        float step = (maxPoint - minPoint) / (float)(totalPlayers - 1);
        int point = Mathf.RoundToInt(maxPoint - (rank - 1) * step);

        return point;
    }
}
