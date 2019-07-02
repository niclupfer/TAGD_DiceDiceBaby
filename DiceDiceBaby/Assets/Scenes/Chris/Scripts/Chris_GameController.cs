using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chris_GameController : MonoBehaviour
{

    public static Chris_GameController gameController;
   
    public Chris_Player Player1;
    public Chris_Player Player2;

    public GameObject Player1Cam;
    public GameObject Player2Cam;

    //draft phase vars
    public static bool pickPhase = true;
    bool playerOnesTurn;
    int dicePicked = 0;
    public Chris_Dice []dicePool;
    public Chris_Dice currentSelected;
    public Text whosPick;
    public Text diceInfo;

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
    }

    public void pickDie()//picking die during drafting
    {
        if (currentSelected != null && pickPhase)
        {
            if (playerOnesTurn)//player ones pick
            {
                Player1.addDice(currentSelected);
                currentSelected = null;
                whosPick.text = "Player Two Choosing";
                diceInfo.text = "";
            }
            else//palyer twos pick
            {
                Player2.addDice(currentSelected);
                currentSelected = null;
                whosPick.text = "Player One Choosing";
                diceInfo.text = "";
            }
            dicePicked++;
            playerOnesTurn = !playerOnesTurn;
            if (dicePicked == 6)//drafting over
            {
                pickPhase = false;
                whosPick.text = "Pick Phase Over";
                diceInfo.text = "";
                Player1Cam.SetActive(true);
                Player2Cam.SetActive(true);
                
            }
            
        }
        
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

    public void deselectDice()//deselects the current die
    {
        foreach (Chris_Dice dice in dicePool)
        {
            dice.deSelect();
        }
        currentSelected = null;
    }

    public void updateDiceInfo()
    {
        diceInfo.text = currentSelected.ToString();
    }//updates dice panel info

    public int getTurn()
    {
        return turnCount;
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
