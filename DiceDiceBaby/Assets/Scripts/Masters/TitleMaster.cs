using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMaster : MonoBehaviour
{
    public string nic_diceTestScene;
    public string nic_lobbySceneName;

    public string chris_draftScene;
    public string chris_diceScene;
    
    public string james_diceScene;

    public void Nic_DiceTest()
    {
        SceneManager.LoadScene(nic_diceTestScene, LoadSceneMode.Single);
    }

    public void Lobby()
    {
        SceneManager.LoadScene(nic_lobbySceneName, LoadSceneMode.Single);
    }

    public void Chris_Dice()
    {
        SceneManager.LoadScene(chris_diceScene, LoadSceneMode.Single);
    }

    public void Chris_Draft()
    {
        SceneManager.LoadScene(chris_draftScene, LoadSceneMode.Single);
    }

    public void James_Dice()
    {
        SceneManager.LoadScene(james_diceScene, LoadSceneMode.Single);
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
