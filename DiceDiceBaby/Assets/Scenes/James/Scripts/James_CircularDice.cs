﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class James_CircularDice : MonoBehaviour
{
    //placed on central DICE object, controls rotation and placement of dice in a ring.
    public float dist = 1f;

    public void PutDiceInRing()
    {
        int dieTot = Chris_GameController.gameController.dicePool.Count;

        //float r = dist/(Mathf.Sin(0.5f * (2 * Mathf.PI / (float)dieTot)));
        for (int d = 0; d < dieTot; d++)
        {
            float angle = d * (2 * Mathf.PI / (float)dieTot);
            //Chris_GameController.gameController.dicePool[d].transform.localPosition = new Vector3(r * Mathf.Cos(angle), r * Mathf.Sin(angle), 0);
            Chris_GameController.gameController.dicePool[d].transform.localPosition = new Vector3(dist * Mathf.Cos(angle), dist * Mathf.Sin(angle), 0);
        }
    }


    public void resetAngle()
    {
        transform.eulerAngles = new Vector3(0, 0, 90);
    }

    public void rotateRight()
    {
        //transform.Rotate(0, 0,(-360f)/ Chris_GameController.gameController.dicePool.Count);
        StartCoroutine(goRight(transform.eulerAngles.z + (-360f) / Chris_GameController.gameController.dicePool.Count));
    }

    private IEnumerator goRight(float f)
    {
        float timePassed = 0;
        while (timePassed <= .5)
        {
            transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(transform.eulerAngles.z, f, timePassed / .5f));
            timePassed += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    public void rotateLeft()
    {
        //transform.Rotate(0, 0, (360f) / Chris_GameController.gameController.dicePool.Count);
        StartCoroutine(goLeft(transform.eulerAngles.z + (360f) / Chris_GameController.gameController.dicePool.Count));
    }

    private IEnumerator goLeft(float f)
    {
        float timePassed = 0;
        while (timePassed <= .5)
        {
            transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(transform.eulerAngles.z, f, timePassed / .5f));
            timePassed += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}
