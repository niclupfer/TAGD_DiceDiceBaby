using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverText : MonoBehaviour
{

    public static GameOverText t = null;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Awake()
    {
        if (t == null)
        {
            t = this;
            DontDestroyOnLoad(t.gameObject);
        }
        else Destroy(this);
    }
    

    public void gameOver(int hp1, int hp2)
    {
        if (hp1 <= 0 && hp2 <= 0) t.text.text = "Game Over \n Tie" ;
        else if (hp1 <= 0) t.text.text = "Game Over \n You Lose";
        else t.text.text = "Game Over \n You Win";
        t.text.enabled = !t.text.enabled;

        SceneManager.LoadScene(1);
    }

    public void disable()
    {
        t.text.enabled = !t.text.enabled;
    }

}
