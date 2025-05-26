using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class Popup_Chatting : BaseWindow
{
    [Header("Input")]
    public TMP_InputField inputField;

    private void Awake()
    {
        inputField.characterLimit = 100;
    }
    
    //엔터로 입력하는 부분 빼버림 (입력버퍼에 엔터가 남는 문제 해결 못함 이슈)
     void Start()
     {
         inputField.onSubmit.AddListener(OnSubmit);
     }

     void OnDestroy()
     {
         inputField.onSubmit.RemoveListener(OnSubmit);
     }
    
     // public void OnEndEdit(string text)  //엔터
     // {
     //     if (!string.IsNullOrWhiteSpace(text))
     //     {
     //         //ChattingSystem.Instance.RPC_SenderChatWrapper(text);
     //         inputField.text = null;                  //입력후 버퍼에 엔터가 남는 문제
     //         // inputField.ForceLabelUpdate();                   // TMP 내부 텍스트 갱신 강제
     //         // inputField.DeactivateInputField();               // 포커스 해제
     //         inputField.ActivateInputField();                 // 다시 입력 가능하게 포커스
     //     }
     // }
    
    private void OnSubmit(string text)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            ChattingSystem.Instance.RPC_SenderChatWrapper(text);

            // 텍스트 정리 + 버퍼 비우기
            StartCoroutine(EnterDeleteCoroutine());
        }
    }
    
    public void OnClickedSubmitButton()  //발송 버튼 클릭
    {
        if (!string.IsNullOrWhiteSpace(inputField.text))
        {
            ChattingSystem.Instance.RPC_SenderChatWrapper(inputField.text);
            
            // 텍스트 정리 + 버퍼 비우기
            StartCoroutine(EnterDeleteCoroutine());
        }
    }

    private IEnumerator EnterDeleteCoroutine() //multyLine에서
    {
        yield return null;
        inputField.text = string.Empty;
        inputField.ActivateInputField();   // 커서 포커스 재위치
    }
}
