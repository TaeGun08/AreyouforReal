using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using Newtonsoft.Json;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager_Network : NetworkBehaviour
{
    private const int MAX_PLAYER = 10;
    public static GameManager_Network Instance { get; private set; }
    [Networked] TickTimer Delay { get; set; }
    
    [Networked, Capacity(MAX_PLAYER)]
    [UnitySerializeField]
    public NetworkLinkedList<LocalPlayer> AlivePlayers { get; }

    [SerializeField] private GameObject map;
    [SerializeField] private GameObject fakeLoading;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private GameObject lounge;
    [SerializeField] private GameObject zone;
    
    [Networked] private GameState delayedState { get; set; }
        
    public enum GameState
    {
        None,
        Wait,
        Start,
        Play,
        End,
    }

    public GameState State
    {
        get => state;
        set => state = value;
    }
    
    [Networked]
    private GameState state { get; set; }

    [Networked] private string CountString { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void Spawned()
    {
        base.Spawned();

        if (Runner.IsSharedModeMasterClient)
        {
            State = GameState.Wait;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GameState.None < delayedState && Delay.ExpiredOrNotRunning(Runner))
        {
            fakeLoading.SetActive(false);
            RPC_ZoneOn();
            // BGameManager.Instance.RPC_InitializeGame();
            state = delayedState;
            delayedState = GameState.None;
        }
    }

    public void TryStartGame()
    {
        Debug.Assert(Runner.IsServer, "서버만 호출가능!");
        SoundManager.Instance.PlayBgm("こんとどぅふぇ素材No.0050-ゲンゲンゲンキだもん");
        Debug.Assert(Runner.IsSharedModeMasterClient, "서버만 호출가능!");
        
        RPC_MapActive();
            
        state = GameState.Start;
        
        // TODO : 로딩페이드
            
        foreach (var player in PlayerRegistry.Instance.playerDic)
        {
            Transform trs = TelpoTransform.Instance.TelepoTrs[Random.Range(0, TelpoTransform.Instance.TelepoTrs.Length)];
                
            // 캐싱필요
            // player.Value.GetComponent<NetworkCharacterController>().Teleport(trs.position);
            RPC_TeleportPlayer(player.Key, trs.position);
            AlivePlayers.Add(player.Value);
            
        }
            
        // TODO : 로딩화면 활성화, 자기장
        for (int i = 0; i < 20; i++)
        {
            AIManager.Instance.SpawnAI(
                TelpoTransform.Instance.TelepoTrs[Random.Range(0, TelpoTransform.Instance.TelepoTrs.Length)].position);
        }
        
        RPC_Count();
        DelaySetState(GameState.Play, 5);
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_MapActive()
    {
        map.SetActive(true);
        fakeLoading.SetActive(true);
        lounge.SetActive(false);
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_Count()
    {
        CountString = $"남은 인원: {AlivePlayers.Count}";
        countText.text = CountString;
    }
    
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_ZoneOn()
    {
        zone.SetActive(true);
    }

    
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    private void RPC_TeleportPlayer([RpcTarget] PlayerRef playerRef, Vector3 position)
    {

        Debug.Log("텔레포트!");
        Player.LocalPlayer.GetComponent<NetworkCharacterController>().Teleport(position);
        
    }
    
    private void DelaySetState(GameState state, float delayTime)
    {
        Delay = TickTimer.CreateFromSeconds(Runner, delayTime);
        delayedState = state;
    }
    
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_KillEvent(LocalPlayer player)
    {
        Debug.Log($"남은 인원: {AlivePlayers.Count}");
        if (AlivePlayers.Remove(player))
        {
            countText.text = $"남은 인원: {AlivePlayers.Count}";
            Debug.Log($"남은 인원: {AlivePlayers.Count}");
            if (AlivePlayers.Count <= 1)
            {
                StartCoroutine(EndCoroutine());
            }
            
            RPC_Count();
        }
    }

    private IEnumerator EndCoroutine()
    {
        yield return new WaitForSeconds(4f);
        MatchHistoryData data = new MatchHistoryData
        {
            PlayerKey = FirebaseMainSession.Instance.FirebaseUser.UserData.UserId,
            Players = PlayerRegistry.Instance.playerDic.Count,
            Rank = Instance.AlivePlayers.Count,
            KillCount = Player.LocalPlayer.GetComponent<PlayerController>().KillCount,
            PlayTime = $"{ZoneSystem.Instance.PlayingTime}",
        };
        
        PlayerPrefs.SetString("SaveHistoryData", JsonConvert.SerializeObject(data));
        yield return new WaitForSeconds(1f);
        
        Runner.Shutdown();
        SceneManager.LoadScene(4);
    }
}
