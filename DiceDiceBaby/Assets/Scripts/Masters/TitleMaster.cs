using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMaster : MonoBehaviour
{
    public string mainGameSceneName;
    public string lobbySceneName;
    public string diceGenSceneName;
    public string diceRollingSceneName;

    public void PlayGame()
    {
        SceneManager.LoadScene(mainGameSceneName, LoadSceneMode.Single);
    }

    public void Test_Lobby()
    {
        SceneManager.LoadScene(lobbySceneName, LoadSceneMode.Single);
    }

    public void Test_DiceGen()
    {
        SceneManager.LoadScene(diceGenSceneName, LoadSceneMode.Single);
    }

    public void Test_DiceRoll()
    {
        SceneManager.LoadScene(diceRollingSceneName, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Quit();
    }
}
