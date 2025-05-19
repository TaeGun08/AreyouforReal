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
    public void StartGame(int playerCount, int aiCount)
    {
        if (currentState == GameState.Ready)
        {
            // 플레이어와 AI를 생성하고 게임 시작
            PlayerManager.Instance.CreatePlayers(playerCount, aiCount);
            currentState = GameState.Playing;
            Debug.Log("게임 시작");
        }
        else
        {
            Debug.Log("오류 발생 현재상태는: " + currentState);
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
            Debug.Log("게임 종류");
        }
        else
        {
            Debug.Log("Game is not running. Cannot end the game.");
        }
    }
    // 현재 상태를 확인하는 메서드
    public GameState GetCurrentState()
    {
        return currentState;
    }
}

