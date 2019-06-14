using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chris_Dice : MonoBehaviour
{
    public Chris_Side[] Sides;
    public Vector3 startPos;
    Rigidbody body;
    bool hasLanded;
    bool hasRolled;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        hasLanded = false;
        hasRolled = false;
        body.useGravity = false;
    }

    private void Update()
    {
        if(hasRolled && !hasLanded && body.IsSleeping())
        {
            hasLanded = true;
            body.useGravity = false;
        }
        else if(hasRolled && hasLanded && body.IsSleeping())
        {
            if(!isSideOnGround())
            {
                hasLanded = false;
                body.useGravity = true;
                body.AddTorque(new Vector3(Random.Range(-1000, 1000), Random.Range(-1000, 1000), Random.Range(-1000, 1000)));
                body.AddForce(new Vector3(Random.Range(-1000, 1000), Random.Range(0, 1000), Random.Range(-1000, 1000)));
            }        
        }
        else if(hasRolled && !body.IsSleeping())body.useGravity = true;
    }

    public void resetDie()
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
        if(!hasRolled && !hasLanded)
        {
            hasRolled = true;
            body.useGravity = true;
            body.AddTorque(new Vector3(Random.Range(-1000, 1000), Random.Range(-1000, 1000), Random.Range(-1000, 1000)));
            body.AddForce(new Vector3(Random.Range(-1000, 1000), Random.Range(0, 1000), Random.Range(-1000, 1000)));
        }
    }

    public bool isSideOnGround()
    {
        for (int i = 0; i < Sides.Length; i++)
        {
            if (Sides[i].faceDown() == true) return true;
        }
        return false;
    }

    public Chris_Side getSideOnGround()
    {
        for (int i = 0; i < Sides.Length; i++)
        {
            if (Sides[i].faceDown() == true) return Sides[i];
        }
        return null;
    }

}
