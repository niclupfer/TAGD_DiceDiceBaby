using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiceMasterState { notSpawned, shaking, rolling, counting, waiting, stashed }

public class DiceMaster : MonoBehaviour
{
    public Dice diceObj;

    public DiceMasterState state = DiceMasterState.notSpawned;

    public int diceCount;

    float diceCheckInterval = .3f; // milliseconds to wait between checking if the dice are done rolling
    float nextThing;

    List<Dice> dice;

    public float gatherRadius;
    public Vector3 gatherPoint;
    public float shakeInterval;

    public float timeScale = 1f;

    void Start()
    {
        dice = new List<Dice>();
    }

    void CreateDice()
    {
        for(int i = 0; i < diceCount; i++)
        {
            var die = Instantiate(diceObj, transform);
            die.transform.position = gatherPoint;
            die.RandomizeSides();
            dice.Add(die);
        }
        state = DiceMasterState.shaking;
    }

    void Update()
    {
        Time.timeScale = timeScale;

        if (state == DiceMasterState.notSpawned && Input.GetKeyDown(KeyCode.Mouse0))
        {
            CreateDice();
        }
        else if (state == DiceMasterState.shaking)
        {
            if (Input.GetKey(KeyCode.Mouse0))
                ShakeDice();
            else
                TossDice();
        }
        else if(state == DiceMasterState.rolling && Time.time > nextThing)
        {
            CheckIfRollingIsDone();            
        }
        else if(state == DiceMasterState.waiting && Input.GetKeyDown(KeyCode.Mouse0))
        {
            state = DiceMasterState.shaking;
        }
    }

    void GatherDice()
    {

    }

    void ShakeDice()
    {
        if(Time.time > nextThing)
        {
            nextThing = Time.time + shakeInterval;
            foreach (var die in dice)
            {
                die.SetTargetOffset((Random.onUnitSphere * gatherRadius));
                die.StartSpin();
            }
        }

        foreach (var die in dice)
        {
            if (!die.HasTargetOffset())
            {
                die.SetTargetOffset(gatherPoint + (Random.onUnitSphere * gatherRadius));
                die.StartSpin();
            }

            die.SpinToOffsetPoint(gatherPoint);
        }
    }

    void TossDice()
    {
        state = DiceMasterState.rolling;
        foreach (var die in dice)
        {
            die.Toss();
        }
    }

    void CheckIfRollingIsDone()
    {
        nextThing = Time.time + diceCheckInterval;
        int doneDice = 0;
        foreach (var die in dice)
        {
            if (die.DoneRolling())
            {
                doneDice++;
            }
            else if (die.Stuck())
            {
                die.Nudge();
            }
        }
        Debug.Log("Done dice: " + doneDice);

        if(doneDice == dice.Count)
        {
            state = DiceMasterState.waiting;
        }
    }
}
