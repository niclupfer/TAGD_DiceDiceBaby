using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Chris_Side
{
    //current Symbol and Value corresponding to it
    public Face Symbol;
    public int Value;

    public override string ToString()
    {
        return Symbol.ToString() + " " + Value; 
    }
}

//enum for what symbols could be on the sides of the dice
public enum Face
{
    Red, Green, Blue, Star, Skull
}