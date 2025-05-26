using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChattingSystem : NetworkBehaviour
{
    public static ChattingSystem Instance { get; private set; }
    
    [Header("Prefabs")]
    [SerializeField] private ChatBubble myBubblePrefab;
    [SerializeField] private ChatBubble otherBubblePrefab;
    
    [Space]
    [Header("Chat List")]
    [SerializeField] private GameObject chatListParent;
    [SerializeField] private ScrollRect scrollRect;
    
    private Queue<ChatBubble> myBubblePool = new Queue<ChatBubble>();
    private Queue<ChatBubble> otherBubblePool = new Queue<ChatBubble>();
    
    private void Awake()
    {
        Instance = this;
        InitializeBubblePool(myBubblePool, myBubblePrefab, 50);
        InitializeBubblePool(otherBubblePool, otherBubblePrefab, 50);
    }
    
    private void InitializeBubblePool(Queue<ChatBubble> pool, ChatBubble prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            ChatBubble bubble = Instantiate(prefab, chatListParent.transform);
            bubble.gameObject.SetActive(false); // 처음엔 비활성화
            pool.Enqueue(bubble);
        }
    }
    
    // public void SendChat(string message, string sender)
    // {
    //     RPC_SendChat(message, sender);
    // }
    //
    // [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    // public void RPC_SendChat(string message, string sender)
    // {
    //     RPC_ReceiveChat(message, sender);
    // }
    
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SenderChatWrapper(string message, RpcInfo info = default)
    {
        RPC_SenderChat(message, info.Source);
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    private void RPC_SenderChat(string message, PlayerRef messageSource)
    {
        Debug.Log("ReceiveChat : " + message);

        if (!Runner.IsRunning)
        {
            Debug.LogWarning("Runner is not running yet.");
            return;
        }
        
        // string sender = FirebaseMainSession.Instance.FirebaseUser.Username;
        
        Debug.Log(messageSource == Runner.LocalPlayer);
        ChatBubble bubble = GetBubbleBySpeaker(messageSource == Runner.LocalPlayer); //자기자신 판별
        
        if(bubble == null) return;
            //bubble.transform.SetParent(chatListParent.transform, false);
            bubble.transform.SetAsLastSibling(); // 맨 아래로 이동
            bubble.idText.text = FirebaseMainSession.Instance.FirebaseUser.Username; // 자신 이름
            bubble.chatText.text = message; // 메시지
            bubble.chatText.text = $"{messageSource == Runner.LocalPlayer}"; // 메시지
            bubble.gameObject.SetActive(true); //활성화
            
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)chatListParent.transform); //레이아웃 갱신
        scrollRect.verticalNormalizedPosition = 0f; //스크롤 내리기
        
        // GameObject chat = Instantiate(bubble, parent: chatContentParent);
        // TMP_Text textComp = bubble.GetComponentInChildren<TMP_Text>();
        //
        // if (textComp == null) return;
        // textComp.text = sender;
        // chatText.text = message;
        // chat.GetComponentInChildren<TMP_Text>().SetText($"{sender}: {message}");
    }
    
    private ChatBubble GetBubbleBySpeaker(bool isMe)
    {
        Queue<ChatBubble> pool = isMe ? myBubblePool :otherBubblePool; //뽑아올 pool 선택. 나 자신 : Other

        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }
        else //풀 부족시 생성
        {
            ChatBubble prefab = isMe ? myBubblePrefab : otherBubblePrefab;
            return Instantiate(prefab, chatListParent.transform);
        }
    }
}
