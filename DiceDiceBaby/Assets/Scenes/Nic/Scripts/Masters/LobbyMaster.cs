using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMaster : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject lobbyScreen;

    public Text statusText;
    public Button readyButt;

    public InputField addressInput;
    public Text gameText;

    public Sprite[] avatars;
    public Image yourAvatar;
    public Image theirAvatar;

    DiceServer myServer;
    DiceClient myClient;
    
    void Start()
    {
        readyButt.GetComponentInChildren<Text>().text = "Not in a game";

        statusText.text = "No game";
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

    void ShowLobby()
    {
        titleScreen.SetActive(false);
        lobbyScreen.SetActive(true);
    }

    public void ShowTitle()
    {
        // just refresh the scene to be safe
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }

    public void ShowAvatars(int yours, int theirs)
    {
        Debug.Log("showing avatars");
        yourAvatar.sprite = avatars[yours + 1];
        theirAvatar.sprite = avatars[theirs + 1];
    }

    public void BattleJoined()
    {
        readyButt.GetComponentInChildren<Text>().text = "Ready?";
        readyButt.interactable = true;
    }

    public void CheckGameStatus(DicePlayer you, DicePlayer them)
    {
        if(you.ready && them.ready)
        {
            statusText.text = "Starting battle";
        }
        else if (!you.ready && them.ready)
        {
            statusText.text = "They are ready, waiting for you";
        }
        else if (you.ready && !them.ready)
        {
            statusText.text = "You are ready, waiting for them";
        }
        else
        {
            statusText.text = "Waiting for both players";
        }
    }

    public void HostGame()
    {
        //statusText.text = "starting host...";

        myServer = new DiceServer(GetComponent<NetworkDiscovery>(), this);
        RefreshGameInfo();

        myClient = new DiceClient(myServer.address, this, false);

        ShowLobby();
    }

    public void JoinGame()
    {
        if (myServer == null)
        {
            var address = addressInput.text;
            statusText.text = "Looking for Battle";
            ConnectClient(address);
            ShowLobby();
        }
    }

    void ConnectClient(string serverIP)
    {
        myClient = new DiceClient(serverIP, this, false);
        
        statusText.text = "Connected to Battle";
    }

    public void ReadyUp()
    {
        myClient.ReadyUp();
    }


    void RefreshGameInfo()
    {
        if (myServer != null)
        {
            gameText.text = "Battle Address/n" + myServer.address;
        }
        else if (myClient != null)
        {
            gameText.text = "Battle Address/n" + myClient.address;
        }
        else
        {
            gameText.text = "No Game Server or Client";
        }
    }

}