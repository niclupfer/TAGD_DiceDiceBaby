using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class James_Spell : ScriptableObject
{
    public new string name;
    //public sprite artwork; //Do we need this?
    public string description;
    public List<cost> costs;

    public James_Enum.damageType dmgType;
    public int amount;
    public int critAmount;
    public int triggerEffect;
}

[System.Serializable]
public struct cost
{
    public Face manaRequirement; // b, r, g, white, black, star, skull
    public int price;
}