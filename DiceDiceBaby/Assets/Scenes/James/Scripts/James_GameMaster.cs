using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

//simulate a battle
public class James_GameMaster : MonoBehaviour
{
    public James_Player[] players;
    public Text[] numbers1; // Blue, Red, Green, Health
    public Text[] numbers2; // Blue, Red, Green, Health

    public James_Spell SpellToFire1;
    public James_Spell SpellToFire2;

    private void Start()
    {
        if(players.Length > 2)
        {
            if (numbers1.Length >= 4)
                numbers1[3].text = players[0].GetHealth().ToString();
            if(numbers2.Length >= 4)
                numbers2[3].text = players[1].GetHealth().ToString();
        }

    }

    private void UpdateNumbers()
    {
        if(players.Length >= 2)
        {
            if (numbers1.Length >= 4)
            {
                numbers1[0].text = players[0].manaPool[0].ToString();
                numbers1[1].text = players[0].manaPool[1].ToString();
                numbers1[2].text = players[0].manaPool[2].ToString();
                numbers1[3].text = players[0].health.ToString();
            }

            if (numbers2.Length >= 4)
            {
                numbers2[0].text = players[1].manaPool[0].ToString();
                numbers2[1].text = players[1].manaPool[1].ToString();
                numbers2[2].text = players[1].manaPool[2].ToString();
                numbers2[3].text = players[1].health.ToString();
            }
        }
    }

    public void FireSpell1()
    {
        players[0].castSpell(SpellToFire1, players[1]);
        UpdateNumbers();
    }

    public void FireSpell2()
    {
        players[1].castSpell(SpellToFire2, players[0]);
        UpdateNumbers();
    }
}
