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
}
