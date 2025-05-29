// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections.Generic;
// using Fusion;
// using TMPro;
//
// public class ChatManager : MonoBehaviour
// {
//     [Header("Prefabs")]
//     public GameObject myBubblePrefab;
//     public GameObject otherBubblePrefab;
//
//     [Header("Parent")]
//     public Transform chatContentParent;
//
//     [Header("Input")]
//     public TMP_InputField inputField;
//
//     private Queue<GameObject> myBubblePool = new Queue<GameObject>();
//     private Queue<GameObject> otherBubblePool = new Queue<GameObject>();
//
//     void Start()
//     {
//         inputField.onSubmit.AddListener(OnSubmit);
//     }
//
//     void OnDestroy()
//     {
//         inputField.onSubmit.RemoveListener(OnSubmit);
//     }
//
//     private void OnSubmit(string text)
//     {
//         if (!string.IsNullOrWhiteSpace(text))
//         {
//             ChattingSystem.Instance.Runner.IsSharedModeMasterClientSenderChatWrapper(text);
//             AddMessage(text);
//             inputField.text = string.Empty;
//         }
//     }
//
//     // public void AddMessage(string message)
//     // {
//     //     GameObject bubble = GetBubbleBySpeaker();
//     //     bubble.transform.SetParent(chatContentParent, false);
//     //     bubble.SetActive(true);
//     //     
//     //     TextMeshProUGUI textComp = bubble.GetComponentInChildren<TextMeshProUGUI>();
//     //     if (textComp != null)
//     //     {
//     //         textComp.text = message;
//     //     }
//     // }
//
//     public GameObject GetBubbleBySpeaker(bool isMe)
//     {
//         Queue<GameObject> pool = isMe ? myBubblePool : otherBubblePool; //뽑아올 pool 선택. 나 자신 : Other
//
//         if (pool.Count > 0)
//         {
//             return pool.Dequeue();
//         }
//         else //풀 부족시 생성
//         {
//             GameObject prefab = isMe ? myBubblePrefab : otherBubblePrefab;
//             return Instantiate(prefab);
//         }
//     }
//
//     public void ReturnBubble(GameObject bubble, string speaker)
//     {
//         bubble.SetActive(false);
//         if (speaker == "Me")
//         {
//             myBubblePool.Enqueue(bubble);
//         }
//         else
//         {
//             otherBubblePool.Enqueue(bubble);
//         }
//     }
//     
//     public void Runner.IsSharedModeMasterClientSendChat(string message, string sender)
//     {
//         RPC_ReceiveChat(message, sender);
//     }
//     
//     [Rpc(RpcSources.All, RpcTargets.All)]
//     private void RPC_ReceiveChat(string message, string sender)
//     {
//         Debug.Log("ReceiveChat : " + message);
//         
//         GameObject bubble = myBubblePool.Dequeue();
//         // GameObject chat = Instantiate(chatPrefab, chatAreaGameObject.transform);
//         // chat.GetComponentInChildren<TMP_Text>().SetText($"{sender}: {message}");
//         
//         // messageList.Add((message, sender));
//     }
// }