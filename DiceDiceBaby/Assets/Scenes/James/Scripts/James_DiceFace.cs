using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class James_DiceFace : ScriptableObject
{
    public new string name;
    public Sprite artwork;

    //FIXME: Pretty sure there's a better way to store the mana types, for now they are in the James_ManaEnum file
    public James_Enum.manaType faceMana;
    public int manaAmount;
}
