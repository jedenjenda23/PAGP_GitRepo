﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFunctions : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
        AudioManager.instance.StopAudio();
        Application.LoadLevel(sceneName);
    }
}
