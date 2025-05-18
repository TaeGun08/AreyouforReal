using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadingSceneManager.LoadScene("LobbyScene");
        }

        startButton.onClick.AddListener(LoadLobbyScene);
    }*/

    public void LoadLobbyScene()
    {
        LoadingSceneManager.LoadScene("LobbyScene");
    }
}