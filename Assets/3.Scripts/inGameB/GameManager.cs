using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GameManager : MonoBehaviour
{
    // 게임 상태 관리
    public enum GameState
    {
        Ready,
        Playing,
        End
    }

    // 현재 게임 상태를 저장하는 변수
    private GameState currentState;

    private void Start()
    {
        ResetGame();
    }

    // 게임 초기화 메서드
    private void ResetGame()
    {
        currentState = GameState.Ready;
        Debug.Log("초기화 및 준비 완료");
    }

    // 게임 시작 메서드
    public void StartGame(int playerCount)
    {
        if (currentState == GameState.Ready)
        {
            // 참가자 수 초기화
            int total = PlayerManager.Instance.humanPlayers.Count 
                        + PlayerManager.Instance.aiPlayers.Count;
            GameResult.Instance.Initialize(total);

            PlayerManager.Instance.CreatePlayers(playerCount);
            PlayerManager.Instance.CreateAIs();

            currentState = GameState.Playing;
            Debug.Log("게임 시작");
        }
        else
        {
            Debug.Log("오류 발생 현재 상태: " + currentState);
        }
    }

    // 게임 종료 메서드
    public void EndGame()
    {
        if (currentState == GameState.Playing)
        {
            // 게임 종료와 동시에 플레이어 데이터 초기화
            PlayerManager.Instance.ClearPlayers();
            currentState = GameState.End;
            Debug.Log("게임 종료");
        }
        else
        {
            Debug.Log("게임이 실행 중이 아닙니다. 종료 불가.");
        }
    }

    // 현재 상태를 확인하는 메서드
    public GameState GetCurrentState()
    {
        return currentState;
    }
}