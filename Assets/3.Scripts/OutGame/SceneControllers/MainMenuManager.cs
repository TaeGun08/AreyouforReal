using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        TouchManager.Instance.OnTouchEnd += LoadLobbyScene;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LoadLobbyScene();
        }
    }

    public void LoadLobbyScene()
    {
        LoadingSceneManager.LoadScene("LobbyScene");
    }
}