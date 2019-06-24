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
    public int dmg;
    public int heal;

    public bool applyShield;
    public int shieldTime;
}

[System.Serializable]
public struct cost
{
    public James_Enum.manaType manaRequirement;
    public int price;
}