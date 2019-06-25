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
    int healthMax = 15;
    int health = 0;

    //ManaVariables
    bool rolledCrit, RolledFail;
    int []manaValues = new int[5];
    public Chris_ManaPanel manaInfo;
    

    //combat variables
    int sheild;
    public James_Spell ChosenSpell;



    private void Start()
    {
        resetManaVariables();
    }

    private void Update()
    {
        diceRolling();
    }

    void diceRolling()
    {
        if (rolledDice && !diceFinishedRolling)
        {
            bool allDiceDown = true;
            for (int i = 0; i < diceInventory.Count; i++)
            {
                if (!diceInventory[i].isSideOnGround() || !diceInventory[i].doneRolling())
                {
                    allDiceDown = false;
                    break;
                }
            }
            if (allDiceDown)
            {
                string rollDebug = "";
                diceFinishedRolling = true;
                foreach (Chris_Dice die in diceInventory)
                {
                    Chris_Side Current = die.getSideOnGround();
                    rollDebug += Current.ToString() + "\n";

                    if (Current.Symbol <= Face.Black)
                    {
                        manaValues[(int)Current.Symbol] += Current.Value;
                    }
                    else if (Current.Symbol == Face.Star) rolledCrit = true;
                    else RolledFail = true;
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

    public void changeHealth(int i, James_Enum.damageType d)
    {
        if (d != James_Enum.damageType.direct)
        {
            health += i;
        }
        else if ((d == James_Enum.damageType.direct) && sheild <= 0)
            health += i;
        checkHealthOverload();
    }

    void checkHealthOverload()
    {
        if (health > healthMax)
            health = healthMax;
    }

    public int getScore()
    {
        return health;
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
        if(sheild > 0)sheild--;
    }

    public void clearInventory()
    {
        diceInventory.Clear();
    }

    public void addDice(Chris_Dice d)
    {
        diceInventory.Add(d);
        d.transform.position = diceLocation - new Vector3(0, 0, 2 * diceInventory.Count - 1); ;
        d.startPos = diceLocation - new Vector3(0,0, 2 * diceInventory.Count - 1);// add an offset so dice are not on top of eachother
    }

    public void chooseSpell(James_Spell spell)
    {
        ChosenSpell = spell;
        foreach (cost c in spell.costs)
        {
            manaValues[(int)(c.manaRequirement)] -= c.price;
        }


        turnFinsihed = true;
    }

}
