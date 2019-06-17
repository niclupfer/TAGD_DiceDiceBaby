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

    int turnCount = 0;
    int maxTurns = 0;

    private void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        gameController = this;//if we have mulitple scenes make this nondestoryable
        startDraft();
        //have it set up the game and start the dice drafting phase
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

    public void pickDie()
    {
        if (currentSelected != null && pickPhase)
        {
            if (playerOnesTurn)
            {
                Player1.addDice(currentSelected);
                currentSelected = null;
                whosPick.text = "Player Two Choosing";
                diceInfo.text = "";
            }
            else
            {
                Player2.addDice(currentSelected);
                currentSelected = null;
                whosPick.text = "Player One Choosing";
                diceInfo.text = "";
            }
            dicePicked++;
            playerOnesTurn = !playerOnesTurn;
            if (dicePicked == 6)
            {
                pickPhase = false;
                whosPick.text = "Pick Phase Over";
                diceInfo.text = "";
                Player1Cam.SetActive(true);
                Player2Cam.SetActive(true);
                
            }
            
        }
        //maybe put dice out on a table and when hovered over shows infromation on them
    }

    public void deselectDice()
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
