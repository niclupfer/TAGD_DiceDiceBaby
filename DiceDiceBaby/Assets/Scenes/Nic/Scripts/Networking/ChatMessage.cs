using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessage : MonoBehaviour
{
    public Text player;
    public Text message;

    public void Init(int playerNum, string msg)
    {
        player.text = "P" + playerNum;

        switch(playerNum)
        {
            case 0:
                player.color = Color.black;
                break;
            case 1:
                player.color = Color.blue;
                break;
            case 2:
                player.color = Color.red;
                break;

            default:
                player.color = Color.yellow;
                break;
        }
        message.text = msg;
    }
}
