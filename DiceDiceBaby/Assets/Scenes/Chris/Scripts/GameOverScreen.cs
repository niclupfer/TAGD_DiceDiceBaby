﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
  
    public void backToMain()
    {
        GameOverText.t.disable();
        SceneManager.LoadScene(0);
    }

}
