using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManager;
    public GameObject attack;
    public GameObject charge;
    public GameObject crit;
    public GameObject healing;
    public GameObject posion;
    public GameObject reflect;
    public GameObject click;

    void Start()
    {
        soundManager = this;
    }

    public void playAttack()
    {
        Instantiate(attack);
    }
    public void playCharge()
    {
        Instantiate(charge);
    }
    public void playCrit()
    {
        Instantiate(crit);
    }
    public void playHeal()
    {
        Instantiate(healing);
    }
    public void playReflect()
    {
        Instantiate(reflect);
    }
    public void playClick()
    {
        Instantiate(click);
    }

}
