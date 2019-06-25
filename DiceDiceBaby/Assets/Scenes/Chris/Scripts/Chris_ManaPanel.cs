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
    public GameObject Crit;
    public GameObject Miss;

    public void updateManaInfo(int[] manaVals, bool crit, bool miss)
    {
        redVal.text = manaVals[0].ToString();
        greenVal.text = manaVals[1].ToString();
        blueVal.text = manaVals[2].ToString();
        whiteVal.text = manaVals[4].ToString();
        blackVal.text = manaVals[3].ToString();
        Crit.SetActive(crit);
        Miss.SetActive(miss);
        this.gameObject.SetActive(true);
    }

    public void resetManaInfo()
    {
        Crit.SetActive(false);
        Miss.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
