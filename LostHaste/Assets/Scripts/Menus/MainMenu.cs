using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartNewGame() {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame() {
        Debug.Log("Quitting Game"); // console log
        Application.Quit();
    }
}
