using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chris_Player : MonoBehaviour
{

    public List<Chris_Dice> diceInventory;
    public Vector3 diceLocation;
    bool rolledDice;
    bool diceFinishedRolling;
    bool turnFinsihed;
    int playerScore = 0;

    //ManaVariables
    bool rolledCrit, RolledFail;
    int []manaValues = new int[5];
    public Chris_ManaPanel manaInfo;

    private void Start()
    {
        for (int i = 0; i < manaValues.Length; i++)
        {
            manaValues[i] = 0;
        }
    }

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
                    Chris_Side Current = die.getSideOnGround();
                    rollDebug += Current.ToString() + "\n";

                    switch (Current.Symbol)
                    {
                        case Face.Red:
                            manaValues[0] += Current.Value;
                            break;
                        case Face.Green:
                            manaValues[1] += Current.Value;
                            break;
                        case Face.Blue:
                            manaValues[2] += Current.Value;
                            break;
                        case Face.Black:
                            manaValues[3] += Current.Value;
                            break;
                        case Face.White:
                            manaValues[4] += Current.Value;
                            break;
                        case Face.Star:
                            rolledCrit = true;
                            break;
                        case Face.Skull:
                            RolledFail = true;
                            break;
                        default:
                            break;
                    }

                }
                Debug.Log(rollDebug);
                manaInfo.updateManaInfo(manaValues, rolledCrit, RolledFail);
            }
            
        }
    }

    void resetManaVariables()
    {
        rolledCrit = false;
        RolledFail = false;
        for (int i = 0; i < manaValues.Length; i++)
        {
            manaValues[i] = 0;
        }
        manaInfo.resetManaInfo();
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
