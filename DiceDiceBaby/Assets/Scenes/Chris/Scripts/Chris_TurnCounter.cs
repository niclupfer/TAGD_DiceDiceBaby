using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chris_TurnCounter : MonoBehaviour
{
    Text counterText;
    
    public void updateCounter()
    {
        counterText.text = Chris_GameController.gameController.getTurn().ToString();
    }

}
