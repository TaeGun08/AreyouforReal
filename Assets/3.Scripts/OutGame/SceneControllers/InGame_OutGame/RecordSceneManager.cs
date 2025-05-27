using System;
using System.Threading.Tasks;
using Firebase.Extensions;
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
    
    // 게임 내 순위, 순위에 따른 포인트증가
    public async Task RecordSceneSetup(string recordRanking, int getRankingPoint)
    {
        FirebaseUser user = FirebaseMainSession.Instance.FirebaseUser;
        
        int currentRankPoint = 0;
        
        userNameText.text = user.Username;
        recordRankingText.text = recordRanking;
        getRankingPointText.text = getRankingPoint.ToString();

        //유저의 현재 랭크포인트를 읽어옴. 데이터 없었을 경우 예외처리
        FirestoreManager.Instance.ReadDataAsync<RankData>(FirebaseCollections.Ranks, user.UserData.UserId).ContinueWithOnMainThread(
            task =>
            {
                RankData rankData = task.Result;
                if (task.IsFaulted || task.IsCanceled)
                {
                    // currentRankPoint = 0;
                    return;
                }
                currentRankPoint =  rankData.RankPoint;
            });
        
        
        userRankingPointText.text = currentRankPoint.ToString();
        
        //쓰기용 데이터 만들기
        RankData rankWriteData = new RankData()
        {
            PlayerName = user.Username,
            RankPoint = currentRankPoint + getRankingPoint
        };

        //업데이트용
        // var rankDic = new Dictionary<string, object> //전달할 딕셔너리
        // {
        //     {"RankPoint", FieldValue.Increment(getRankingPoint)} // 필드, 값 형태로 전달시 일치하는 필드의 값을 변경
        // };

        //업데이트 없이 덮어쓰기 => RankData 없는 경우 오류 방지하고 새로쓰기
        await FirestoreManager.Instance.WriteDataAsync(FirebaseCollections.Ranks, user.UserData.UserId, rankWriteData)
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
}
