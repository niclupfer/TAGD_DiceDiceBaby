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

    public int yourPlayerNum;

    public Chris_GameController gameController;
    public GameObject draftingContainer;

    public AudioClip battleMusic;
    public AudioSource backgroundMusic;

    public GameObject clickSFX;
    
    void Start()
    {
        readyButt.GetComponentInChildren<Text>().text = "Not in a game";

        statusText.text = "No game";
    }

    public void ResetAll()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
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
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void ShowAvatars(int yours, int theirs)
    {
        Debug.Log("showing avatars");
        yourAvatar.sprite = avatars[yours + 1];
        theirAvatar.sprite = avatars[theirs + 1];
    }

    public void ShowDiceDrafting()
    {
        Debug.Log("showing drafting");
        lobbyScreen.SetActive(false);
        draftingContainer.SetActive(true);
        backgroundMusic.clip = battleMusic;
        backgroundMusic.Play();
    }

    public void BattleJoined()
    {
        yourPlayerNum = myClient.playerNum;

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

    public void GenerateDice()
    {
        ShowDiceDrafting();

        gameController.startDraft();

        if (myServer == null)
            Debug.Log("generate dice, big error, no server");

        myServer.SendDicePool(gameController.GetDicePool(), NotYou());

        myServer.SendDiceTurn(gameController.WhoPicksFirst());
    }


    int NotYou()
    {
        if (yourPlayerNum == 1)
            return 2;
        else if (yourPlayerNum == 2)
            return 1;

        return -1; // Error if this
    }

    public void SetDicePool(string diceData)
    {
        ShowDiceDrafting();
        gameController.SetDicePool(diceData);
    }

    public void SetDiceTurn(int whosTurn)
    {
        gameController.SetTurn(whosTurn, yourPlayerNum);
    }

    public void EnemyPicked(string dice)
    {
        gameController.enemyPickDie(dice);
    }

    public void IPickedDie(string diceName)
    {
        myClient.SendDicePick(diceName);
    }

    public void HeresMyMana(string manaData)
    {
        myClient.SendMyMana(manaData);
    }

    public void LearnEnemyMana(string manaData)
    {
        gameController.updateEnemyInfo(manaData);
    }

    public void ImCastingSpell(string spellData)
    {
        myClient.SendSpell(spellData);
    }

    public void EnemyCastSpell(string spellData)
    {
        gameController.getEnemySpellInfo(spellData);
    }

    public void HeresMyInfo(string playerInfo)
    {
        myClient.SendMyInfo(playerInfo);
    }

    public void EnemyInfo(string enemyInfo)
    {
        gameController.getEnemyInfo(enemyInfo);
    }

    public void HostGame()
    {
        //statusText.text = "starting host...";

        myServer = new DiceServer(GetComponent<NetworkDiscovery>(), this);
        RefreshGameInfo();

        myClient = new DiceClient(myServer.address, this, false);

        ShowLobby();

        Instantiate(clickSFX);
    }

    public void JoinGame()
    {
        if (myServer == null)
        {
            var address = addressInput.text;
            statusText.text = "Looking for Battle";
            ConnectClient(address);
            ShowLobby();

            Instantiate(clickSFX);
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
            gameText.text = "Battle Address \n" + myServer.address;
        }
        else if (myClient != null)
        {
            gameText.text = "Battle Address \n" + myClient.address;
        }
        else
        {
            gameText.text = "No Game Server or Client";
        }
    }

}