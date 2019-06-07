using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chris_GameController : MonoBehaviour
{
    public GameObject dicePanelHolder;
    public GameObject dicePanelPrefab;
    public Chris_Player Player;
    public GameObject []dicePanels;

    public void rollPlayerDice()
    {
        foreach (GameObject item in dicePanels)
        {
            Destroy(item);
        }
        Chris_Side []Rollded = Player.rollInventory();
        dicePanels = new GameObject[Rollded.Length];
        for (int i = 0; i < Rollded.Length; i++)
        {
            dicePanels[i] = Instantiate(dicePanelPrefab);
            Text t = dicePanels[i].GetComponentInChildren<Text>();
            t.text = Rollded[i].ToString();
            dicePanels[i].transform.SetParent(dicePanelHolder.transform);
        }
    }

}
