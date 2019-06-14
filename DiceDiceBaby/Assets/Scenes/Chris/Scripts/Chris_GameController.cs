using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chris_GameController : MonoBehaviour
{
    
    public Chris_Player Player1;
    public Chris_Player Player2;

    int turnCount = 0;
    int maxTurns = 0;

    private void Start()
    {
        //have it set up the game and start the dice drafting phase
    }

    private void diceDraft()
    {
        //bring up dice for drafting allowing player to alternate taking dice
    }

    public void rollPlayerDice(Chris_Player P)
    {
        P.rollInventory();
    }

    public void resetPlayerDice(Chris_Player P)
    {
        P.resetInventory();
    }

}
