using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chris_GameController : MonoBehaviour
{

    public static Chris_GameController gameController;
    public James_CircularDice DiceCircle;
    public GameObject dicePoolLocation;
   
    public Chris_Player Player;
    public GameObject PlayerCam;
    public GameObject PlayerUI;
    public bool roundFinished = false;
    public bool enemySpellChosen = false;
    public bool enemyManaRecived = false;
    string enemySpellData = "";
    int turnCount = 1;
    int maxTurns = 3;
    public Chris_ManaPanel enemyInfo;
    

    //draft phase vars
    public static bool pickPhase = true;
    public bool yourTurn;
    int dicePicked = 0;
    public List<Chris_Dice> dicePool;
    public GameObject d6Prefab;
    public GameObject d8Prefab;
    public GameObject d12Prefab;
    public GameObject d20Prefab;
    public ScriptableDice []d6Dice;
    public ScriptableDice []d8Dice;
    public ScriptableDice []d12Dice;
    public ScriptableDice []d20Dice;
    public GameObject draftCanvas;
    public GameObject draftCam;
    public DiceInfoPanel[] infoPanels;
    public TextMeshProUGUI diceName;
    public int currentDie;
    public Text whosPick;

    public LobbyMaster lobby;
    
    //temp variabls for testing
    public GameObject nextTurnButtion;

    private void Awake()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        gameController = this;//if we have mulitple scenes make this nondestoryable
        //startDraft();
        //have it set up the game and start the dice drafting phase
    }

    private void Start()
    {
        //startDraft();
    }

    private void Update()
    {
        if (dicePool[currentDie] != null) dicePool[currentDie].transform.Rotate(1, 1, 1);

        if (roundFinished) nextTurn(); // we would tell the game to call the calculate functons here to update health for spells chosen

        if(Player.diceFinishedRolling && enemyManaRecived)//both players have rolled start spell select
        {
            Player.showSpellList();
        }

        if(Player.spellCast && enemySpellChosen)// player has processed their spell, process the enemies spell
        {
            Player.processMySpell();
            Player.processEnemySpell(enemySpellData);
        }
    }

    //Drafting functions

    public void startDraft()
    {
        //bring up dice for drafting allowing player to alternate taking dice
        populateDicePool();// if the player is the host they will need to popualte the draft then somehow send the info the the enemy;

        currentDie = 0;
    }

    public int WhoPicksFirst()
    {
        //if host send signal for whos picking first;
        return Random.Range(0, 2) + 1;
    }

    public void SetTurn(int playerNum, int yourNum)
    {
        if (playerNum == yourNum)
            yourTurn = true;

        if (playerNum == 1)
        {
            whosPick.text = "Player One Choosing";
        }
        else
        {
            whosPick.text = "Player Two Choosing";
        }
    }

    public void pickDie()//picking die during drafting
    {
        if (dicePool[currentDie] != null && pickPhase)
        {
            if (yourTurn)//player ones pick
            {
                //send data to enemy for what dice you chose
                string diceName = dicePool[currentDie].diceInfo.name;
                Player.addDice(dicePool[currentDie]);
                dicePool.RemoveAt(currentDie);
                whosPick.text = "Other Player Choosing";
                yourTurn = false;
                checkDicePhaseState();

                lobby.IPickedDie(diceName);
            }
        }
        
    }

    public void enemyPickDie(string diceName)//would be called by the listener fucntion according to what string is recived
    {
        if(!yourTurn)
        {
            for (int i = 0; i < dicePool.Count; i++)
            {
                if (dicePool[i].diceInfo.id.Equals(diceName))
                {
                    //dicePool[i].transform.position = new Vector3(100 * dicePool.Count, 100, 100);
                    Destroy(dicePool[i].gameObject);
                    dicePool.RemoveAt(i);
                    whosPick.text = "Your Pick";
                    yourTurn = true;
                    break;
                }
            }
            checkDicePhaseState();
        }

    }

    public void checkDicePhaseState()
    {
        dicePicked++;
        if (dicePicked == 6)//drafting over
        {
            pickPhase = false;
            whosPick.text = "Pick Phase Over";
            PlayerCam.SetActive(true);
            PlayerUI.SetActive(true);
            draftCam.SetActive(false);
            draftCanvas.SetActive(false);
            //disable draft came and gui and swtch player to their screen
        }
        else
        {
            DiceCircle.resetAngle();
            DiceCircle.PutDiceInRing();
            currentDie = 0;
            updateDiceInfo();
        }
    }

    public void shiftRight()
    {
        if (currentDie == dicePool.Count - 1)
        {
            currentDie = 0;
        }
        else
        {
            currentDie++;
        }
        updateDiceInfo();
        DiceCircle.rotateRight();
    }

    public void shiftLeft()
    {
        if (currentDie == 0)
        {
            currentDie = dicePool.Count - 1;
        }
        else
        {
            currentDie--;
        }
        updateDiceInfo();
        DiceCircle.rotateLeft();
    }

    public void updateDiceInfo()
    {
        foreach (DiceInfoPanel panel in infoPanels)
        {
            panel.disable();
        }
        Chris_Side[] sides = dicePool[currentDie].Get_Sides();
        for (int i = 0; i < sides.Length; i++)
        {
            infoPanels[i].info = sides[i];
            infoPanels[i].updateInfo();
        }
        diceName.text = dicePool[currentDie].diceInfo.id;

    }//updates dice panel info

    private void populateDicePool()
    {
        //Only host should do this?

        dicePool.Clear();
        for (int i = 0; i < 10; i++)
        {
            GameObject newDie;
            ScriptableDice newDiceInfo;
            int r = Random.Range(0, 3);
            if (r == 0)
            {
                int random = Random.Range(0, d6Dice.Length - 1);
                newDie = Instantiate(d6Prefab,dicePoolLocation.transform);
                newDiceInfo = d6Dice[random];
                Debug.Log("D6 Index " + random);
            }
            else if (r == 1)
            {
                int random = Random.Range(0, d8Dice.Length - 1);
                newDie = Instantiate(d8Prefab, dicePoolLocation.transform);
                newDiceInfo = d8Dice[random];
                Debug.Log("D8 Index " + random);
            }
            else if(r == 2)
            {
                int random = Random.Range(0, d12Dice.Length - 1);
                newDie = Instantiate(d12Prefab, dicePoolLocation.transform);
                newDiceInfo = d12Dice[random];
                Debug.Log("D12 Index " + random);
            }
            else
            {
                int random = Random.Range(0, d20Dice.Length - 1);
                newDie = Instantiate(d20Prefab, dicePoolLocation.transform);
                newDiceInfo = d20Dice[random];
                Debug.Log("D20 Index " + random);
            }
            Chris_Dice dieComponent = newDie.GetComponent<Chris_Dice>();
            dieComponent.diceInfo = newDiceInfo;
            dieComponent.updateSides();
            dicePool.Add(dieComponent);
        }

        currentDie = 0;
        updateDiceInfo();
        DiceCircle.resetAngle();
        DiceCircle.PutDiceInRing();

    }

    public string GetDicePool()
    {
        string pool = "";
        for(int i = 0; i < dicePool.Count - 1; i++)
        {
            pool += dicePool[i].diceInfo.name + ",";
        }
        pool += dicePool[dicePool.Count - 1].diceInfo.name;

        Debug.Log("pool string: ");
        Debug.Log(pool);
        //send the string?
        return pool;
    }

    public void SetDicePool(string data)//look through dice pools looking for names? or have a hashtable with key being the name and data being the dice info
    {
        Debug.Log("Setting dice pool: " + data);
        dicePool.Clear();
        string[] pool = data.Split(',');
        foreach (string s in pool)
        {
            if(s.Substring(0,1) == "6")
            {
                Chris_Dice D = Instantiate(d6Prefab, dicePoolLocation.transform).GetComponent<Chris_Dice>();
                for(int i = 0; i < d6Dice.Length;i++)
                {
                    if(d6Dice[i].name.Equals(s))
                    {
                        D.diceInfo = d6Dice[i];
                        break;
                    }
                }
                D.updateSides();
                dicePool.Add(D);
            }
            else if (s.Substring(0, 1) == "8")
            {
                Chris_Dice D = Instantiate(d8Prefab, dicePoolLocation.transform).GetComponent<Chris_Dice>();
                for (int i = 0; i < d8Dice.Length; i++)
                {
                    if (d8Dice[i].name.Equals(s))
                    {
                        D.diceInfo = d8Dice[i];
                        break;
                    }
                }
                D.updateSides();
                dicePool.Add(D);
            }
            else if (s.Substring(0, 2) == "12")
            {
                Chris_Dice D = Instantiate(d12Prefab, dicePoolLocation.transform).GetComponent<Chris_Dice>();
                for (int i = 0; i < d12Dice.Length; i++)
                {
                    if (d12Dice[i].name.Equals(s))
                    {
                        D.diceInfo = d12Dice[i];
                        break;
                    }
                }
                D.updateSides();
                dicePool.Add(D);
            }
            else if (s.Substring(0, 2) == "20")
            {
                Chris_Dice D = Instantiate(d20Prefab, dicePoolLocation.transform).GetComponent<Chris_Dice>();
                for (int i = 0; i < d20Dice.Length; i++)
                {
                    if (d20Dice[i].name.Equals(s))
                    {
                        D.diceInfo = d20Dice[i];
                        break;
                    }
                }

                dicePool.Add(D);
            }
        }

        Debug.Log("num of dice: " + dicePool.Count);
        currentDie = 0;
        updateDiceInfo();
        DiceCircle.resetAngle();
        DiceCircle.PutDiceInRing();
    }

    //game fuctions

    public int getTurn()
    {
        return turnCount;
    }

    public void updateEnemyInfo(string data)
    {
        string[] manaInfo = data.Split(',');

        int[] manaVals = new int[6];

        for (int i = 0; i < manaInfo.Length - 1; i++)
        {
            manaVals[i] = int.Parse(manaInfo[i]);
        }
        if (manaInfo[5].Equals("0"))
        {
            enemyInfo.updateManaInfo(manaVals, false);
        }
        else enemyInfo.updateManaInfo(manaVals, true);

    }//will occure after players roll to show eachothers mana and at the end of the turn to show updated health and sheild values

    public void getEnemySpellInfo(string data)
    {
        enemySpellData = data;
        enemySpellChosen = true;
    }

    public void nextTurn()//reset everything for next turn
    {
        turnCount++;
        if (turnCount == maxTurns + 1) endGame();//end game if turn count maxed
        else//reset
        {
            Player.resetInventory();
            enemyInfo.resetManaInfo();
            roundFinished = false;
            enemySpellChosen = false;
            enemyManaRecived = false;
        }
    }

    public void endGame()
    {
        roundFinished = false;
        //ending the game happens here;
        Debug.Log("GameOver");
    }

    //Roll fucntions

    public void rollPlayerDice(Chris_Player P)
    {
        P.rollInventory();
    }

    public void resetPlayerDice(Chris_Player P)
    {
        P.resetInventory();
    }

}
