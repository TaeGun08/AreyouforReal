using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ChatManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject myBubblePrefab;
    public GameObject otherBubblePrefab;

    [Header("Parent")]
    public Transform chatContentParent;

    [Header("Input")]
    public TMP_InputField inputField;

    private Queue<GameObject> myBubblePool = new Queue<GameObject>();
    private Queue<GameObject> otherBubblePool = new Queue<GameObject>();

    void Start()
    {
        inputField.onSubmit.AddListener(OnSubmit);
    }

    void OnDestroy()
    {
        inputField.onSubmit.RemoveListener(OnSubmit);
    }

    private void OnSubmit(string text)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            AddMessage(text, "Me");
            inputField.text = string.Empty;
        }
    }

    public void AddMessage(string message, string speaker)
    {
        GameObject bubble = GetBubbleBySpeaker(speaker);
        bubble.transform.SetParent(chatContentParent, false);
        bubble.SetActive(true);

        TextMeshProUGUI textComp = bubble.GetComponentInChildren<TextMeshProUGUI>();
        if (textComp != null)
        {
            textComp.text = message;
        }
    }

    private GameObject GetBubbleBySpeaker(string speaker)
    {
        bool isMe = speaker == "Me";
        Queue<GameObject> pool = isMe ? myBubblePool : otherBubblePool;
        GameObject prefab = isMe ? myBubblePrefab : otherBubblePrefab;

        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }
        else
        {
            return Instantiate(prefab);
        }
    }

    public void ReturnBubble(GameObject bubble, string speaker)
    {
        bubble.SetActive(false);
        if (speaker == "Me")
        {
            myBubblePool.Enqueue(bubble);
        }
        else
        {
            otherBubblePool.Enqueue(bubble);
        }
    }
}