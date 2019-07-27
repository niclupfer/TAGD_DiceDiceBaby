using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatView : MonoBehaviour
{
    public ChatMessage chatMsgObj;

    RectTransform lastMessage;

    public float msgHeight;
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void AddMessage(int playerNum, string msg)
    {
        var yPos = 0f;
        if(lastMessage != null)
        {
            yPos = lastMessage.anchoredPosition3D.y - msgHeight;
        }

        var newMsg = Instantiate(chatMsgObj, transform);
        newMsg.Init(playerNum, msg);
        lastMessage = newMsg.GetComponent<RectTransform>();
        lastMessage.anchoredPosition3D = new Vector3(0, yPos, 0);

        var height = Mathf.Abs(yPos - msgHeight);
        rect.sizeDelta = new Vector2(0, height);
        rect.anchoredPosition3D = new Vector3(0, (height * 0.5f) + 15, 0);
    }    
}
