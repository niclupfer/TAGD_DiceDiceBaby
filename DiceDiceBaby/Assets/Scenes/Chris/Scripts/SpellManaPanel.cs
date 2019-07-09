using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpellManaPanel : MonoBehaviour
{

    public Sprite Red;
    public Sprite Blue;
    public Sprite Green;
    public Sprite White;
    public Sprite Black;
    public Sprite Crit;

    public Image sprite;
    public TextMeshProUGUI value;

    public void updateCost(cost c)
    {
        switch (c.manaRequirement)
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
        }

        value.text = c.price.ToString();

        gameObject.SetActive(true);

    }

    public void disable()
    {
        gameObject.SetActive(false);
    }
}
