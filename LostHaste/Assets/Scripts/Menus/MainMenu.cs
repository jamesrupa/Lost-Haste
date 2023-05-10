using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Button LoadGameBTN;

    private void Start() {
        LoadGameBTN.onClick.AddListener(() =>
        {
            SaveManager.Instance.StartLoadedGame();
        }
        );
    }

    public void StartNewGame() {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame() {
        Debug.Log("Quitting Game"); // console log
        Application.Quit();
    }
}
