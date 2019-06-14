using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chris_Side : MonoBehaviour
{
    //current Symbol and Value corresponding to it
    public Face Symbol;
    public int Value;
    bool onGround;

    private void Start()
    {
        onGround = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor")) onGround = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Floor")) onGround = false;
    }

    public bool faceDown()
    {
        return onGround;
    }

    public Chris_Side getSide()
    {
        return this;
    }

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