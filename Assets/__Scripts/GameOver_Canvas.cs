using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver_Canvas : MonoBehaviour
{
    [Header("Set Dynamically")]
    public float score;
    public Text scoreText;
    void Start()
    {
        score = PlayerPrefs.GetFloat("Score");
        scoreText = transform.Find("Score").gameObject.GetComponent<Text>();
        scoreText.text = "Your team scored: " + Mathf.RoundToInt(score).ToString("D") + " points!";
    }
}
