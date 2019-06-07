using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chris_Player : MonoBehaviour
{

    public Chris_Dice []diceInventory;

    public Chris_Side[] rollInventory()
    {
        Chris_Side[] rolledSides = new Chris_Side[diceInventory.Length];
        for (int i = 0; i < diceInventory.Length; i++)
        {
            rolledSides[i] = diceInventory[i].rollDice();
        }
        return rolledSides;
    }

}
