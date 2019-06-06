using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * replace the given text with a random string from an array - Nic L.
 */
public class RandomTitleChooser : MonoBehaviour
{
    public string[] possibleTitles;

    void Start()
    {
        GetComponent<Text>().text = TAGD.ChooseRandom(possibleTitles);   
    }
}
