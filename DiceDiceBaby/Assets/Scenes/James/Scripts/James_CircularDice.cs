using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class James_CircularDice : MonoBehaviour
{
    //placed on central DICE object, controls rotation and placement of dice in a ring.
    public Chris_GameController gameController;

    private int dieTot;

    private void Start()
    {
        dieTot = gameController.dicePool.Count;
    }

    void PutDiceInRing()
    {
        float r = Mathf.Sin(0.5f * (360f/(float)dieTot));
        for (int d = 0; d < dieTot; d++)
        {
            Transform dicePlace = gameController.dicePool[d].transform;
            float angle = d * (360f / (float)dieTot);
            dicePlace.localPosition = new Vector3(r * Mathf.Cos(angle),0,r*Mathf.Sin(angle));
        }
    }
}
