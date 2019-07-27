using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chris_ManaPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public Text redVal;
    public Text blueVal;
    public Text greenVal;
    public Text blackVal;
    public Text whiteVal;
    public Text critVal;
    public GameObject Miss;

    public void updateManaInfo(int[] manaVals, bool miss)
    {
        redVal.text = manaVals[1].ToString();
        greenVal.text = manaVals[2].ToString();
        blueVal.text = manaVals[0].ToString();
        whiteVal.text = manaVals[3].ToString();
        blackVal.text = manaVals[4].ToString();
        critVal.text = manaVals[5].ToString();
        Miss.SetActive(miss);
        this.gameObject.SetActive(true);
    }

    public void resetManaInfo()
    {
        Miss.SetActive(false);
        redVal.text = "0";
        greenVal.text = "0";
        blueVal.text = "0";
        whiteVal.text = "0";
        blackVal.text = "0";
        critVal.text = "0";
        //this.gameObject.SetActive(false);
    }
}
