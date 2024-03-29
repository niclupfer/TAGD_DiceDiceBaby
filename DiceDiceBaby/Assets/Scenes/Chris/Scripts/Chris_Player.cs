﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chris_Player : MonoBehaviour
{

    public static Chris_Player player;

    public List<Chris_Dice> diceInventory;
    public Vector3 diceLocation;
    bool rolledDice;
    public bool diceFinishedRolling;
    int healthMax = 15;
    int health = 0;

    //ManaVariables
    bool rolledCrit, RolledFail;
    int []manaValues = new int[6]; // b, r, g, white, black, star, skull
    public Chris_ManaPanel manaInfo;


    //combat variables
    public bool spellListUp = false;
    public bool spellCast = false;
    int sheild = 0;
    int triggerEffect = 1;
    int addedDamage = 0;
    public James_Spell ChosenSpell;
    public SpellCostPanel SpellList; //currently set to a buttion
    public GameObject RollButtion;

    //heath and shield info
    public TextMeshProUGUI healthValue;
    public TextMeshProUGUI sheildValue;
    public TextMeshProUGUI stackedValue;
    public TextMeshProUGUI triggerValue;

    public LobbyMaster lobby;

    private void Start()
    {
        resetManaVariables();
        player = this;
        health = healthMax;
        sendPlayerData();
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
                manaInfo.updateManaInfo(manaValues, RolledFail);//update ui
                sendManaInfo();
                //send mana info to enemy through message
                //showSpellList();//show the spell list for picking
            }
        }
    }

    private void sendManaInfo()
    {
        string s = "";
        for (int i = 0; i < manaValues.Length; i++)
        {
            s += manaValues[i] + ",";
        }
        if (RolledFail == true) s += "0";
        else s += "1";
        Debug.Log("Sending Enemy Mana: " + s);
        //send
        lobby.HeresMyMana(s);
    }

    public void showSpellList()
    {
        //sort spell player can choose from
        if(RolledFail)//dont show spell list
        {
            processMySpell();
        }
        else SpellList.activate(manaValues);//display for them to pick
        spellListUp = true;
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

    public void changeHealth(int i, James_Enum.damageType d)
    {
        Debug.Log("changing my health : " + i);
        if (d == James_Enum.damageType.poison)
        {
            health -= i;
        }
        else if ((d == James_Enum.damageType.direct) && sheild <= 0)
            health -= i;
        else if (d == James_Enum.damageType.healing) health += i;
        checkHealthOverload();
        Debug.Log("my health " + health);
    }

    void checkHealthOverload()
    {
        if (health > healthMax) health = healthMax;
    }

    public int getHealth()
    {
        return health;
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
        spellListUp = false;
        spellCast = false;
        resetManaVariables();
        foreach (Chris_Dice die in diceInventory)
        {
            die.resetDie();
        }
        RollButtion.SetActive(true);
    }

    public void clearInventory()
    {
        diceInventory.Clear();
    }

    public void addDice(Chris_Dice d)
    {
        d.body.isKinematic = false;
        d.transform.SetParent(this.transform);
        d.startPos = this.transform.position - new Vector3(0, 0, 2 * diceInventory.Count - 1);
        d.resetDie();
        diceInventory.Add(d);
    }

    public void chooseSpell(James_Spell spell)
    {
        ChosenSpell = spell;//select as chosen spell
        ChosenSpell.critAmount = manaValues[5];
        ChosenSpell.triggerEffect = triggerEffect;
        processMySpell();//process spell
    }

    public void processMySpell()
    {
        string spellString = "";
        if (RolledFail == false)
        {
            if (ChosenSpell.name.Substring(0, 6).Equals("Attack"))//attack spell
            {
                Debug.Log("I'm Attacking");
                ChosenSpell.amount += addedDamage;//put added dmg in
                spellString = "Attack," + ChosenSpell.amount + "," + triggerEffect + "," + ChosenSpell.critAmount; //dmg = (basedmg + crit dmg) * trigger effect 
                                                                                                                   //reset trigger effect and added dmg
                addedDamage = 0;
                triggerEffect = 1;

                SoundManager.soundManager.playAttack();
            }
            else if (ChosenSpell.name.Substring(0, 5).Equals("Boost"))//add dmg to next dmg
            {
                Debug.Log("I'm Boosting");
                for (int i = 0; i < triggerEffect; i++)//trigger i amount of times
                {
                    addedDamage += ChosenSpell.amount + 3 * ChosenSpell.critAmount;//Boost = amount + ctit * 3
                }
                spellString = "Boost," + ChosenSpell.amount + "," + triggerEffect + "," + ChosenSpell.critAmount;
                triggerEffect = 1;

                SoundManager.soundManager.playCharge();
            }
            else if (ChosenSpell.name.Substring(0, 4).Equals("Heal"))//healing
            {
                Debug.Log("I'm healing");
                for (int i = 0; i < triggerEffect; i++)//trigger i amount of times
                {
                    changeHealth(ChosenSpell.amount + 3 * ChosenSpell.critAmount, ChosenSpell.dmgType);// healing = amount + crit * 3
                }
                spellString = "Heal," + ChosenSpell.amount + "," + triggerEffect + "," + ChosenSpell.critAmount;
                triggerEffect = 1;

                SoundManager.soundManager.playHeal();
            }
            else if (ChosenSpell.name.Substring(0, 6).Equals("Repeat"))//trigger effect
            {
                Debug.Log("I'm repeating");
                int newTriggerEffect = 0;//temp for the new tigger effect 
                for (int i = 0; i < triggerEffect; i++)//trigger i amount of times
                {
                    addedDamage += 3 * ChosenSpell.critAmount;//if crit get more added dmg
                    newTriggerEffect += ChosenSpell.amount;//add up trigger effect incase stacking 
                }
                spellString = "Repeat," + ChosenSpell.amount + "," + triggerEffect + "," + ChosenSpell.critAmount;
                triggerEffect = newTriggerEffect;

                SoundManager.soundManager.playCharge();
            }
            else if (ChosenSpell.name.Substring(0, 6).Equals("Shield"))
            {
                Debug.Log("I'm shielding");
                for (int i = 0; i < triggerEffect; i++)//trigger i amount of times
                {
                    sheild += ChosenSpell.amount;
                    if (ChosenSpell.critAmount > 0) changeHealth(ChosenSpell.critAmount * 3, James_Enum.damageType.healing);//if crit get health
                }
                spellString = "Shield," + ChosenSpell.amount + "," + triggerEffect + "," + ChosenSpell.critAmount;
                triggerEffect = 1;

                SoundManager.soundManager.playReflect();
            }
        }
        else spellString = "Fail";

        //send out data String
        lobby.ImCastingSpell(spellString);
        spellCast = true;//Player spell done Proccesing let enemy spell go though in game controller
    }

    public void sendPlayerData()
    {
        healthValue.text = health.ToString();
        sheildValue.text = sheild.ToString();
        stackedValue.text = addedDamage.ToString();
        triggerValue.text = triggerEffect.ToString();
        string send = health.ToString() + "," + sheild.ToString() + "," + addedDamage.ToString() + "," + triggerEffect.ToString();

        //send it
        lobby.HeresMyInfo(send);
    }

    public void processEnemySpell(string data)
    {
        //Attack,totalDamage,triggerEffect,critVal
        //Heal,totalAmount,triggerEffect,critVal
        //Shield,totalAmount,triggerEffect,critVal
        //Boost,totalAmount,triggerEffect,critVal
        //Repeat,totalAmount,triggerEffect,critVal
        Debug.Log("EnemySpellData: " + data);
        if(!data.Equals("Fail"))
        {
            string[] spellData = data.Split(',');
            if (spellData[0] == "Attack")
            {
                int totalDamage = (int.Parse(spellData[1]) + int.Parse(spellData[3])) * int.Parse(spellData[2]); //should crit go through trigger effect?

                //playAnimation maybe do it trigger effect amount of times with dmg vals popping up
                SoundManager.soundManager.playAttack();
                changeHealth(totalDamage, James_Enum.damageType.direct);
            }
            else if (spellData[0] == "Heal") SoundManager.soundManager.playHeal();//do animation/sound?
            else if (spellData[0] == "Shield") SoundManager.soundManager.playReflect();
            else if (spellData[0] == "Boost") SoundManager.soundManager.playCharge();
            else if (spellData[0] == "Repeat") SoundManager.soundManager.playCharge(); ;
        }
        if (sheild > 0) sheild--;

        sendPlayerData();


        Chris_GameController.gameController.roundFinished = true;//signal to game the next turn can happen
    }

}
