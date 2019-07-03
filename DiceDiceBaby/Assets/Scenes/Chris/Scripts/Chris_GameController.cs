using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chris_GameController : MonoBehaviour
{

    public static Chris_GameController gameController;
    public James_CircularDice DiceCircle;

   
    public Chris_Player Player1;
    public Chris_Player Player2;

    public GameObject Player1Cam;
    public GameObject Player2Cam;

    //draft phase vars
    public static bool pickPhase = true;
    bool playerOnesTurn;
    int dicePicked = 0;
    public List<Chris_Dice> dicePool;
    public DiceInfoPanel[] infoPanels;
    public TextMeshProUGUI diceName;
    public int currentDie;
    public Text whosPick;

    int turnCount = 1;
    int maxTurns = 3;

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
        if (nextTurnButtion.active == false && Player1.getTurnFinished() == true && Player2.getTurnFinished() == true && turnCount != maxTurns + 1) nextTurnButtion.SetActive(true);//testing for next turn buttion
    }

    //Drafting functions

    private void startDraft()
    {
        //bring up dice for drafting allowing player to alternate taking dice
        int coinFlip = Random.Range(0, 2);
        Debug.Log(coinFlip);
        if (coinFlip == 0)
        {
            playerOnesTurn = true;
            whosPick.text = "Player One Choosing";
        }
        else
        {
            playerOnesTurn = false;
            whosPick.text = "Player Two Choosing";
        }

        currentDie = 0;
        updateDiceInfo();
        DiceCircle.resetAngle();
        DiceCircle.PutDiceInRing();
    }

    public void pickDie()//picking die during drafting
    {
        if (dicePool[currentDie] != null && pickPhase)
        {
            if (playerOnesTurn)//player ones pick
            {
                Player1.addDice(dicePool[currentDie]);
                dicePool.RemoveAt(currentDie);
                whosPick.text = "Player Two Choosing";
            }
            else//palyer twos pick
            {
                Player2.addDice(dicePool[currentDie]);
                dicePool.RemoveAt(currentDie);
                whosPick.text = "Player One Choosing";
            }
            dicePicked++;
            playerOnesTurn = !playerOnesTurn;
            if (dicePicked == 6)//drafting over
            {
                pickPhase = false;
                whosPick.text = "Pick Phase Over";
                Player1Cam.SetActive(true);
                Player2Cam.SetActive(true);
                
            }
            else
            {
                DiceCircle.resetAngle();
                DiceCircle.PutDiceInRing();
                currentDie = 0;
                updateDiceInfo();
            }
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

    public void nextTurn()//reset everything for next turn
    {
        turnCount++;
        if (turnCount == maxTurns + 1) endGame();//end game if turn count maxed
        else//reset
        {
            Player1.resetInventory();
            Player2.resetInventory();
        }
    }

    public void endGame()
    {
        ////change to helth 
        if (Player1.getScore() > Player2.getScore()) Debug.Log("Player One wins!");
        else if (Player1.getScore() < Player2.getScore()) Debug.Log("Player Two wins!");
        else Debug.Log("Tie~~!");
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
