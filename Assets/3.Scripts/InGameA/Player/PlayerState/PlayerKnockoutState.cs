using System.Collections;
using System.Collections.Generic;
using Fusion;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerKnockoutState : PlayerState
{
    public override State CurrentState => State.Knockout;
    
    public override void StateEnter(PlayerController playerController)
    {
        SoundManager.Instance.PlayBgm("maou_se_battle16");
        
        this.playerController = playerController;
        this.playerController.ChangeAnimation(CurrentState);

        GameManager_Network.Instance.KillEvent(this.playerController.LocalPlayer);

        //GameResult.Instance.RecordElimination(this.playerController);
        
        StartCoroutine(RecordSceneLoadCoroutine());
    }

    private IEnumerator RecordSceneLoadCoroutine()
    {
        MatchHistoryData data = new MatchHistoryData
        {
            PlayerKey = FirebaseMainSession.Instance.FirebaseUser.UserData.UserId,
            Players = new List<string>(),
            Rank = GameManager_Network.Instance.AlivePlayers.Count,
            KillCount = playerController.KillCount,
            PlayTime = $"{BGameManager.Instance.ZoneUI.ElapsedGameTime}",
        };
        
        //ToDo 하랑 할 일 
        PlayerPrefs.SetString("SaveHistoryData", JsonConvert.SerializeObject(data));
        
        //Json 불러오는 방법
        //if (string.IsNullOrEmpty(PlayerPrefs.GetString("SaveHistoryData"))) return;
        //MatchHistoryData data = JsonConvert.DeserializeObject<MatchHistoryData>(PlayerPrefs.GetString("SaveHistoryData"));
        
        yield return new WaitForSeconds(3f);
        playerController.Runner.Shutdown();
        SceneManager.LoadSceneAsync(4);
    }

    public override void StateUpdate()
    {
    }

    public override void StateExit()
    {
    }
}
