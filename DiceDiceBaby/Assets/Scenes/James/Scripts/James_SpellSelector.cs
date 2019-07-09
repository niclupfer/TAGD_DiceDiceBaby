using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class James_SpellSelector : MonoBehaviour
{
    public Text name;
    public James_Spell spell;


    private void Start()
    {
        name.text = spell.name;
    }

    public void enable()
    {
        gameObject.SetActive(true);
    }

    public void disable()
    {
        gameObject.SetActive(false);
    }

    /*
    public void SetSpell()
    {
        //player = FindPlayer(transform).GetComponent<James_Player>();
        //if(player != null)
       // {
       //     Debug.Log("Set " + player.name + " spell to " + spell.name + "!");
       //     player.selectedSpell = spell;
       // }
    }

    Transform FindPlayer(Transform p)
    {
        if(p.GetComponent<James_Player>() == null && transform.IsChildOf(transform))
        {
            p = FindPlayer(p.transform.parent);
        }else if(p.GetComponent<James_Player>() == null)
        {
            Debug.Log("Cannot find parent player!");
            return p = null;
        }
        return p;
    }
    */
}
