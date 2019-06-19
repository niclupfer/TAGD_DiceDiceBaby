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

    public static T ChooseRandom_Weighted<T>(T[] array, int[] weights)
    {
        List<T> weightedList = new List<T>();

        for (var i = 0; i < array.Length; i++)
        {
            for (var k = 0; k < weights[i]; k++)
            {
                weightedList.Add(array[i]);
            }
        }

        var weightedArr = weightedList.ToArray();

        return weightedArr[Random.Range(0, weightedArr.Length)];
    }
}