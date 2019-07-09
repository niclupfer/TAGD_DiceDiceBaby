using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCostPanel : MonoBehaviour
{
    public Chris_Player player;
    public List<SpellManaPanel> spellMPanels;

    public void UpdateSpellPanels()
    {
        foreach(SpellManaPanel smp in spellMPanels)
        {
            smp.disable();
        }
        for(int i = 0; i < player.ChosenSpell.costs.Count; i++)
        {
            spellMPanels[i].updateCost(player.ChosenSpell.costs[i]);
        }
    }
}
