using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Chris_Dice : MonoBehaviour
{
    //rolling variables
    public Chris_Side[] Sides;
    public ScriptableDice diceInfo;
    public Vector3 startPos;
    Rigidbody body;
    bool hasLanded;
    bool hasRolled;

    //drafting variables
    bool isSelected = false;


    private void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        body = GetComponent<Rigidbody>();//set up variables and states
        hasLanded = false;
        hasRolled = false;
        body.useGravity = false;

        //put info into sides
        for (int i = 0; i < Sides.Length; i++)
        {
            Sides[i].info = diceInfo.Sides[i];
            Sides[i].updateSprite();
        }

    }

    private void Update()
    {
        if(hasRolled && !hasLanded && body.IsSleeping())//if dice has stopped moving after having been rolled its landed
        {
            hasLanded = true;
            body.useGravity = false;
        }
        else if(hasRolled && hasLanded && body.IsSleeping())//if the dice has landed and is not moving
        {
            if(!isSideOnGround())//check if at lease one side is on the ground if not launch itself
            {
                hasLanded = false;
                body.useGravity = true;
                body.AddTorque(new Vector3(Random.Range(-1000, 1000), Random.Range(-1000, 1000), Random.Range(-1000, 1000)));
                body.AddForce(new Vector3(Random.Range(-1000, 1000), Random.Range(0, 1000), Random.Range(-1000, 1000)));
            }        
        }
        else if(hasRolled && !body.IsSleeping())body.useGravity = true;//else if floating in space just land
    }

    public void resetDie()//go abck to roll posistion
    {
        body.useGravity = false;
        body.MovePosition(startPos);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        body.velocity = new Vector3(0, 0, 0);
        body.angularVelocity = new Vector3(0, 0, 0);
        hasLanded = false;
        hasRolled = false;
    }

    public void rollDice()
    {
        if(!hasRolled && !hasLanded)//if hasent been rolled roll it
        {
            hasRolled = true;
            body.useGravity = true;
            body.AddTorque(new Vector3(Random.Range(-1000, 1000), Random.Range(-1000, 1000), Random.Range(-1000, 1000)));
            body.AddForce(new Vector3(Random.Range(-1000, 1000), Random.Range(0, 1000), Random.Range(-1000, 1000)));
        }
    }

    public bool isSideOnGround()//check if a side is on the ground
    {
        for (int i = 0; i < Sides.Length; i++)
        {
            if (Sides[i].faceDown() == true) return true;
        }
        return false;
    }

    public bool doneRolling()//is the dice done rolling?
    {
        return body.IsSleeping();
    }

    public Chris_Side getSideOnGround()//return the side thats on the ground
    {
        for (int i = 0; i < Sides.Length; i++)
        {
            if (Sides[i].faceDown() == true) return Sides[i];
        }
        return null;
    }

    /***********************************************Drafting functions Start*********************************/
    private void OnMouseDown()
    {
        //open a dice info screen, show a select buttion
        if(!isSelected && !hasRolled && Chris_GameController.pickPhase)
        {
            //deselect all other dice
            Chris_GameController.gameController.deselectDice();
            isSelected = true;
            //show dice infomation
            Chris_GameController.gameController.currentSelected = this;
            Chris_GameController.gameController.updateDiceInfo();
            //allow select
            
        }
    }

    public void deSelect()
    {
        isSelected = false;
    }

    public override string ToString()
    {
        string info = "";
        foreach (Chris_Side side in Sides)
        {
            info += side.ToString() + "\n";
        }
        return info;
    }

}
