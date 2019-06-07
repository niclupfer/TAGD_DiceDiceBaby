using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Dice")]
public class Chris_Dice : ScriptableObject
{
    public Chris_Side[] Sides;

    public Chris_Side rollDice()
    {
        int selectedSide = Random.Range(0, Sides.Length);
        return Sides[selectedSide];
    }

}
