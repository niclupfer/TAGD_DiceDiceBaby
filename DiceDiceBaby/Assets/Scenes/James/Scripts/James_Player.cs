using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class James_Player : MonoBehaviour
{
    //leaving as a monobehaviour for now
    public string playerName;
    public int health = 100;
    public bool shield = false;

    public int[] manaPool; // blue, red, green, crit, miss, white, black
    public List<James_DiceBody> diceDraft;
    public James_Player opponent;

    public string GetPlayerName()
    {
        return this.playerName;
    }

    void resetManaPool()
    {
        for(int i = 0; i < manaPool.Length; i++)
        {
            manaPool[i] = 0;
        }
    }

    #region Health Stuff
    public int GetHealth()
    {
        return this.health;
    }

    public void changeHealth(int i, James_Enum.damageType d)
    {
        if (d != James_Enum.damageType.direct)
        {
            health += i;
        }
        else if ((d == James_Enum.damageType.direct) && !shield)
            health += i;
        checkHealthOverload();
    }

    void checkHealthOverload()
    {
        if (health > 100)
            health = 100;
    }
    #endregion

    public void addDice(James_DiceBody d)
    {
        diceDraft.Add(d);
    }

    public void castSpell(James_Spell s, James_Player target)
    {
        foreach(cost c in s.costs)
        {
            manaPool[(int)(c.manaRequirement)] -= c.price;
        }

        target.changeHealth(-s.dmg, s.dmgType);
        this.changeHealth(+s.heal, James_Enum.damageType.healing);
    }
}
