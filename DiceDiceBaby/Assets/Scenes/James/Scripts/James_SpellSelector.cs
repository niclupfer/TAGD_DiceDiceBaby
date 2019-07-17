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

}
