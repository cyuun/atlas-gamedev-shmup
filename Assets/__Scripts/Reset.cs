﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Scenes/_Scene_0");
    }
}
