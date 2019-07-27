using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chris_Player : MonoBehaviour
{

    public static Chris_Player player;

    public List<Chris_Dice> diceInventory;
    public Vector3 diceLocation;
    bool rolledDice;
    bool diceFinishedRolling;
    bool turnFinsihed;
    int healthMax = 15;
    int health = 0;

    //ManaVariables
    bool rolledCrit, RolledFail;
    int []manaValues = new int[6]; // b, r, g, white, black, star, skull
    public Chris_ManaPanel manaInfo;
    

    //combat variables
    int sheild;
    public James_Spell ChosenSpell;
    public SpellCostPanel SpellList; //currently set to a buttion
    public GameObject RollButtion;



    private void Start()
    {
        resetManaVariables();
        player = this;
    }

    private void Update()
    {
        diceRolling();
    }

    void diceRolling()
    {
        if (rolledDice && !diceFinishedRolling)//if dice are rolling
        {
            bool allDiceDown = true;
            for (int i = 0; i < diceInventory.Count; i++)//look to see if al dice are down and done rolling
            {
                if (!diceInventory[i].isSideOnGround() || !diceInventory[i].doneRolling())
                {
                    allDiceDown = false;
                    break;
                }
            }
            if (allDiceDown)//if done
            {
                string rollDebug = "";
                diceFinishedRolling = true;
                foreach (Chris_Dice die in diceInventory)//fill mana in data
                {
                    Chris_Side Current = die.getSideOnGround();
                    rollDebug += Current.ToString() + "\n";

                    if (Current.info.symbol <= Face.Star)
                    {
                        manaValues[(int)Current.info.symbol] += Current.info.value;
                    }
                    else RolledFail = true;
                    if (Current.info.symbol == Face.Star) rolledCrit = true;
                    
                }
                Debug.Log(rollDebug);
                manaInfo.updateManaInfo(manaValues, rolledCrit, RolledFail);//update ui
                //send mana info to enemy through message
                showSpellList();//show the spell list for picking
            }
        }
    }

    void showSpellList()
    {
        //sort spell player can choose from
        SpellList.activate(manaValues);//display for them to pick
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
        RollButtion.SetActive(true);
    }

    public void clearInventory()
    {
        diceInventory.Clear();
    }

    public void addDice(Chris_Dice d)
    {
        d.body.isKinematic = false;
        d.startPos = diceLocation - new Vector3(0, 0, 2 * diceInventory.Count - 1);
        d.resetDie();
        diceInventory.Add(d);
    }

    public void chooseSpell(James_Spell spell)
    {
        ChosenSpell = spell;//select as chosen spell
        foreach (cost c in spell.costs)//take mana away 
        {
            manaValues[(int)(c.manaRequirement)] -= c.price;
        }


        turnFinsihed = true;
    }

}
