using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chris_GameController : MonoBehaviour
{

    public static Chris_GameController gameController;
    public James_CircularDice DiceCircle;

   
    public Chris_Player Player;
    public GameObject PlayerCam;
    public GameObject PlayerUI;
    bool enemyFinished = false;
    int turnCount = 1;
    int maxTurns = 3;
    public Chris_ManaPanel enemyInfo;
    

    //draft phase vars
    public static bool pickPhase = true;
    bool yourTurn;
    int dicePicked = 0;
    public List<Chris_Dice> dicePool;
    public GameObject draftCanvas;
    public GameObject draftCam;
    public DiceInfoPanel[] infoPanels;
    public TextMeshProUGUI diceName;
    public int currentDie;
    public Text whosPick;

    
    //temp variabls for testing
    public GameObject nextTurnButtion;


    private void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        gameController = this;//if we have mulitple scenes make this nondestoryable
        startDraft();
        //have it set up the game and start the dice drafting phase
    }

    private void Update()
    {
        if (dicePool[currentDie] != null) dicePool[currentDie].transform.Rotate(1, 1, 1);

        if (Player.getTurnFinished() && enemyFinished) nextTurn(); // we would tell the game to call the calculate functons here to update health for spells chosen
    }

    //Drafting functions

    private void startDraft()
    {
        //bring up dice for drafting allowing player to alternate taking dice
        int coinFlip = Random.Range(0, 2);
        Debug.Log(coinFlip);
        if (coinFlip == 0)
        {
            yourTurn = true;
            whosPick.text = "Player One Choosing";
        }
        else
        {
            yourTurn = false;
            whosPick.text = "Player Two Choosing";
        }

        //PopulateDraft() if the player is the host they will need to popualte the draft then somehow send the info the the enemy;

        //if host send signal for whos picking first;
        currentDie = 0;
        updateDiceInfo();
        DiceCircle.resetAngle();
        DiceCircle.PutDiceInRing();
    }

    public void pickDie()//picking die during drafting
    {
        if (dicePool[currentDie] != null && pickPhase)
        {
            if (yourTurn)//player ones pick
            {
                //send data to enemy for what dice you chose
                Player.addDice(dicePool[currentDie]);
                dicePool.RemoveAt(currentDie);
                whosPick.text = "Player Two Choosing";
                yourTurn = false;
                checkDicePhaseState();
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
                    dicePool[i].transform.position = new Vector3(100, 100, 100);
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

    //game fucntions

    public int getTurn()
    {
        return turnCount;
    }

    public void updateDraftInfo(string data)
    {
  
    }//Script to recive the dice that should be in the draft for both players

    public void enemyTurnFinish(string data)
    {
        //would be for choosen spell info
        enemyFinished = true;
    }//at the end of an enemys turn they could send info about the spell they used

    void updateEnemyInfo(string data)
    {

        //would be for updating what the enemy rolled

    }//will occure after players roll to show eachothers mana and at the end of the turn to show updated health and sheild values

    public void nextTurn()//reset everything for next turn
    {
        turnCount++;
        if (turnCount == maxTurns + 1) endGame();//end game if turn count maxed
        else//reset
        {
            Player.resetInventory();
            enemyFinished = false;
        }
    }

    public void endGame()
    {
        enemyFinished = false;//temp
        ////change to helth 
        //if (Player1.getScore() > Player2.getScore()) Debug.Log("Player One wins!");
        //else if (Player1.getScore() < Player2.getScore()) Debug.Log("Player Two wins!");
        //else Debug.Log("Tie~~!");
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
