using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void startGame ()
    {
        SceneManager.LoadScene(1);
    }

    public void exitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
