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

    //FIXME: effect? Some effects are very complex, might need to make a different class for them.
    //public James_Enum.dmgType dmgType; //the type of damage?
    public int dmg; //positive for damage, negative for healing.
    public int delay; //How many turns to wait before effect happens?
}

[System.Serializable]
public struct cost
{
    public James_Enum.manaType manaRequirement;
    public int price;
}