using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chris_Side : MonoBehaviour
{
    public Sprite Red;
    public Sprite Blue;
    public Sprite Green;
    public Sprite White;
    public Sprite Black;
    public Sprite Fail;
    public Sprite Crit;

    //current Symbol and Value corresponding to it
    public sideInfo info;
    public SpriteRenderer sprite;
    bool onGround;

    private void Start()
    {
        onGround = false;
    }//initalize
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor")) onGround = true;
    }//for checking which side is down

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Floor")) onGround = false;
    }

    public void updateSprite()//set the sprite accoring to values
    {
        //set sprite according to face
        switch(info.symbol)
        {
            case Face.Red:
                sprite.sprite = Red;
                break;
            case Face.Blue:
                sprite.sprite = Blue;
                break;
            case Face.Green:
                sprite.sprite = Green;
                break;
            case Face.Black:
                sprite.sprite = Black;
                break;
            case Face.White:
                sprite.sprite = White;
                break;
            case Face.Star:
                sprite.sprite = Crit;
                break;
            case Face.Skull:
                sprite.sprite = Fail;
                break;
        }
    }

    public bool faceDown()
    {
        return onGround;
    }//returns if the face is down

    public Chris_Side getSide()
    {
        return this;
    }//get the side

    public override string ToString()
    {
        return info.symbol.ToString() + " " + info.value;
    }
}

//enum for what symbols could be on the sides of the dice
public enum Face 
{
    Blue, Red, Green, White, Black, Star, Skull
}

[System.Serializable]
public struct sideInfo//side info for makeing scriptable dice
{
    public Face symbol;
    public int value;
}