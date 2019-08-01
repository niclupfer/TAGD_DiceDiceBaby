using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTetser : MonoBehaviour
{
    public GameObject menuThemeObject;
    public GameObject battleThemeObject;

    public GameObject attackObj;
    public GameObject poisonObj;
    public GameObject criticalObj;
    public GameObject reflectObj;
    public GameObject healObj;
    public GameObject chargeObj;
    public GameObject scrollObj;

    GameObject menuTheme;
    GameObject battleTheme;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (menuTheme != null)
            {
                Destroy(menuTheme);
                menuTheme = null;
            }
            else
            {
                Destroy(battleTheme);
                battleTheme = null;

                menuTheme = Instantiate(menuThemeObject);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (battleTheme != null)
            {
                Destroy(battleTheme);
                battleTheme = null;
            }
            else
            {
                Destroy(menuTheme);
                menuTheme = null;

                battleTheme = Instantiate(battleThemeObject);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Instantiate(attackObj);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            Instantiate(poisonObj);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            Instantiate(healObj);
        if (Input.GetKeyDown(KeyCode.Alpha6))
            Instantiate(reflectObj);
        if (Input.GetKeyDown(KeyCode.Alpha7))
            Instantiate(criticalObj);
        if (Input.GetKeyDown(KeyCode.Alpha8))
            Instantiate(chargeObj);
        if (Input.GetKeyDown(KeyCode.Alpha9))
            Instantiate(scrollObj);
    }
}
