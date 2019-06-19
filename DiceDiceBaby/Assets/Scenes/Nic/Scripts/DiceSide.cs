using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSide : MonoBehaviour
{
    public Sprite mana_Red;
    public Sprite mana_Blue;
    public Sprite mana_Green;
    public Sprite mana_White;
    public Sprite mana_Black;
    public Sprite mana_Success;
    public Sprite mana_Fail;


    SpriteRenderer[] spriteRens;
    
    void Awake()
    {
        spriteRens = GetComponentsInChildren<SpriteRenderer>();
    }

    public void SetMana(char[] manas)
    {
        if(spriteRens.Length == manas.Length)
        {
            for(int i = 0; i < spriteRens.Length; i++)
            {
                spriteRens[i].sprite = GetSpriteFor(manas[i]);
            }
        }
        else
        {
            Debug.LogError("DiceSide wasnt given enough sprites");
        }
    }

    Sprite GetSpriteFor(char m)
    {
        switch(m)
        {
            case 'r':
                return mana_Red;
            case 'g':
                return mana_Green;
            case 'b':
                return mana_Blue;
            case 'w':
                return mana_White;
            case 'u':
                return mana_Black;
            case 's':
                return mana_Success;
            case 'f':
                return mana_Fail;

        }

        return mana_Fail;
    }

    public int ManaSlots()
    {
        return spriteRens.Length;
    }

    public void PutOnSide(int sideNum)
    {
        switch(sideNum)
        {
            // Top
            case 0:
                transform.localPosition = new Vector3(0, 0.51f, 0);
                transform.localEulerAngles = new Vector3(90, 0, 0);
                break;

            // Bottom
            case 1:
                transform.localPosition = new Vector3(0, -0.51f, 0);
                transform.localEulerAngles = new Vector3(-90, 0, 0);
                break;

            // Left
            case 2:
                transform.localPosition = new Vector3(-0.51f, 0, 0);
                transform.localEulerAngles = new Vector3(0, 90, 0);
                break;

            // Right
            case 3:
                transform.localPosition = new Vector3(0.51f, 0, 0);
                transform.localEulerAngles = new Vector3(0, -90, 0);
                break;

            // Front
            case 4:
                transform.localPosition = new Vector3(0, 0, 0.51f);
                transform.localEulerAngles = new Vector3(0, 180, 0);
                break;

            // Back
            case 5:
                transform.localPosition = new Vector3(0, 0, -0.51f);
                transform.localEulerAngles = new Vector3(0, 0, 0);
                break;
        }
    }
}
