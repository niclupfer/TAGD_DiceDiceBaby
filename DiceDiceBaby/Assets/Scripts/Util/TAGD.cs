using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Static class to hold static helper functions - Nic L.
 */ 
public static class TAGD
{
    public static Color TRANSPARENT = new Color(0, 0, 0, 0);

    public static T ChooseRandom<T>(T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
}