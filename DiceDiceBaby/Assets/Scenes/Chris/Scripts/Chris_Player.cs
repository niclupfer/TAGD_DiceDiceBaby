using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chris_Player : MonoBehaviour
{

    public List<Chris_Dice> diceInventory;
    bool rolledDice;
    bool diceFinishedRolling;
    bool turnFinsihed;
    public Vector3 diceLocation;
    int playerScore = 0;

    //ManaVariables
    bool rolledCrit, RolledFail;
    int red, blue, green, white, black;

    private void Update()
    {
        if(rolledDice && !diceFinishedRolling)
        {
            bool allDiceDown = true;
            for (int i = 0; i < diceInventory.Count; i++)
            {
                if(!diceInventory[i].isSideOnGround() || !diceInventory[i].doneRolling())
                {
                    allDiceDown = false;
                    break;
                }
            }
            if (allDiceDown)
            {
                string rollDebug = "";
                diceFinishedRolling = true;
                turnFinsihed = true;
                foreach (Chris_Dice die in diceInventory)
                {
                    rollDebug += die.getSideOnGround().ToString() + "\n";
                    //needs to add up the values of the sides and put them in the mana vars to be used for choosing spells
                }
                Debug.Log(rollDebug);
            }
            
        }
    }

    void resetManaVariables()
    {
        rolledCrit = false;
        RolledFail = false;
        red = 0;
        blue = 0;
        green = 0;
        black = 0;
        white = 0;
    }

    public bool getTurnFinished()
    {
        return turnFinsihed;
    }

    public void addScore(int i)
    {
        playerScore += i;
    }

    public int getScore()
    {
        return playerScore;
    }
    
    public void rollInventory()
    {
        rolledDice = true;
        foreach (Chris_Dice die in diceInventory)
        {
            die.rollDice();
        }
    }

    public void resetInventory()
    {
        rolledDice = false;
        diceFinishedRolling = false;
        turnFinsihed = false;
        resetManaVariables();
        foreach (Chris_Dice die in diceInventory)
        {
            die.resetDie();
        }
    }

    public void clearIncentory()
    {
        diceInventory.Clear();
    }

    public void addDice(Chris_Dice d)
    {
        diceInventory.Add(d);
        d.transform.position = diceLocation - new Vector3(0, 0, 2 * diceInventory.Count - 1); ;
        d.startPos = diceLocation - new Vector3(0,0, 2 * diceInventory.Count - 1);// add an offset so dice are not on top of eachother
    }

}
