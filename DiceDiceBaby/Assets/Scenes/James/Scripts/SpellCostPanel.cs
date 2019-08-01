using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellCostPanel : MonoBehaviour
{
    public TextMeshProUGUI SpellName;
    public TextMeshProUGUI SpellDesc;
    public List<SpellManaPanel> spellMPanels;
    public James_Spell currentSpell;
    public RectTransform scrollField;

    public List<James_SpellSelector> spells;//maybe we refreance all the spells here and enable them according to mana given

    private void Start()
    {
        foreach (SpellManaPanel smp in spellMPanels)
        {
            smp.disable();
        }
    }


    public void UpdateSpellPanels(James_SpellSelector s)
    {
        currentSpell = s.spell;
        SpellName.text = s.spell.name;
        SpellDesc.text = s.spell.description;
        foreach(SpellManaPanel smp in spellMPanels)
        {
            smp.disable();
        }
        for(int i = 0; i < s.spell.costs.Count; i++)
        {
            spellMPanels[i].updateCost(s.spell.costs[i]);
        }
    }

    public void castSpell()
    {
        Chris_Player.player.chooseSpell(currentSpell);
        gameObject.SetActive(false);
    }

    public void activate(int []manaVals)
    {
        foreach (James_SpellSelector spell in spells)
        {
            spell.disable();
        }
        int count = 0;
        foreach(James_SpellSelector spell in spells)
        {
            bool spellPassed = true;
            for(int i = 0; i < spell.spell.costs.Count; i++)
            {
                Debug.Log(manaVals[(int)spell.spell.costs[i].manaRequirement]);
                if (manaVals[(int)spell.spell.costs[i].manaRequirement] < spell.spell.costs[i].price)
                {
                    spellPassed = false;
                }
            }
            if (spellPassed)
            {
                spell.enable();
                count++;
            }
        }

        scrollField.sizeDelta = new Vector2(scrollField.sizeDelta.x, count * 35 + 5);

        this.gameObject.SetActive(true);
    }
    
}
