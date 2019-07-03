using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceInfoPanel : MonoBehaviour
{
    public Chris_Side info;
    public Image sprite;
    public Text text;

    public void updateInfo()
    {

        sprite.sprite = info.sprite.sprite;
        text.text = info.info.value.ToString();
        this.gameObject.SetActive(true);
    }

    public void disable()
    {
        this.gameObject.SetActive(false);
    }
}
