using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chris_Player : MonoBehaviour
{

    public List<Chris_Dice> diceInventory;
    bool rolledDice;
    bool diceFinishedRolling;
    public Vector3 diceLocation;

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
                diceFinishedRolling = true;
                foreach (Chris_Dice die in diceInventory)
                {
                    Debug.Log(die.getSideOnGround().ToString());
                }

            }
            
        }
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
