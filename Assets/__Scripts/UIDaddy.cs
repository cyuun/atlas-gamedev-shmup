using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIDaddy : MonoBehaviour
{
    public static UIDaddy S;
    
    [Header("Set in Inspector")]
    public int maxWrongEnemies = 5;
    public GameObject wrongEnemyPrefab;
    public float bgScrollSpeed = 0.5f;

    [Header("Set Dynamically")]
    public bool gameIsPlaying = false;
    public float timeLeft = 60f;
    public Text scoreText;
    public Text timerText;
    public int[] wrongEnemies = new[] {0, 0, 0};
    public GameObject[,] capsules = new GameObject[3,5];
    [SerializeField]
    private int _score = 0;
    public float score
    {
        get { return (float)_score; }
        set
        {
            _score = Mathf.FloorToInt(value);
            scoreText.text = "Team Score: " + score.ToString();
        }
    }

    private Renderer bgRender;
    private RawImage startScreenImage;

    void Awake()
    {
        S = this;
        startScreenImage = transform.Find("Canvas/StartScreen").gameObject.GetComponent<RawImage>();
        scoreText = transform.Find("Canvas/Score").gameObject.GetComponent<Text>();
        timerText = transform.Find("Canvas/Timer").gameObject.GetComponent<Text>();
        bgRender = transform.Find("Background").gameObject.GetComponent<Renderer>();
        if (gameIsPlaying == false)
        {
            scoreText.color = new Color(1, 1, 1, 0);
            timerText.color = new Color(1, 1, 1, 0);
            startScreenImage.color = new Color(1, 1, 1, 0.8f);
        }
        else
        {
            scoreText.color = new Color(1, 1, 1, 0.8f);
            timerText.color = new Color(1, 1, 1, 0.8f);
            startScreenImage.color = new Color(1, 1, 1, 0);
        }
    }

    private void Update()
    {
        float offset = Time.time * bgScrollSpeed;
        ScrollBG(offset);
        if (timeLeft > 0)
        {
            if (gameIsPlaying == true)
            {
                UpdateTimer(Time.deltaTime);
            }
        }
        else
        {
            PlayerPrefs.SetFloat("Score", score);
            SceneManager.LoadScene("Scenes/_Scene_GameOver");
        }

        if (gameIsPlaying == false && Input.GetKeyDown(KeyCode.Space))
        {
            gameIsPlaying = true;
            scoreText.color = new Color(1, 1, 1, 0.8f);
            timerText.color = new Color(1, 1, 1, 0.8f);
            startScreenImage.color = new Color(1, 1, 1, 0);
            Main.S.StartPlaying();
        }
    }

    public void IncrementWrongEnemy(int identifierNum)
    {
        wrongEnemies[identifierNum] += 1;
        if (wrongEnemies[identifierNum] >= 5)
        {
            ResetCapsules();
            wrongEnemies[identifierNum] = 0;
        }
        else if (wrongEnemies[identifierNum] > 0)
        {
            GameObject go = Instantiate<GameObject>(wrongEnemyPrefab);
            capsules[identifierNum, wrongEnemies[identifierNum] - 1] = go;
            if (go.gameObject.GetComponent<Renderer>() != null)
            {
                go.gameObject.GetComponent<Renderer>().material = Main.S.materials[identifierNum];
            }
            go.transform.position = Main.S.wrongEnemyCapsulePositions[identifierNum, wrongEnemies[identifierNum] - 1];
            go.transform.SetParent(this.transform,true);
        }
    }

    private void ResetCapsules()
    {
        foreach (var go in capsules)
        {
            if (go != null)
            {
                Destroy(go);
            }
        }
        int inputSwitchIndex = Random.Range(0, 3);
        Hero.S0.ChangeInputKeys(Main.S.heroKeySets[inputSwitchIndex,0], Main.S.heroKeySets[inputSwitchIndex,1], Main.S.heroKeySets[inputSwitchIndex,2]);
        Hero.S1.ChangeInputKeys(Main.S.heroKeySets[(inputSwitchIndex + 1) % 3,0], Main.S.heroKeySets[(inputSwitchIndex + 1) % 3,1], Main.S.heroKeySets[(inputSwitchIndex + 1) % 3,2]);
        Hero.S2.ChangeInputKeys(Main.S.heroKeySets[(inputSwitchIndex + 2) % 3,0], Main.S.heroKeySets[(inputSwitchIndex + 2) % 3,1], Main.S.heroKeySets[(inputSwitchIndex + 2) % 3,2]);
    }

    private void ScrollBG(float offset)
    {
        Vector2 offsetVector = new Vector2(offset, 0);
        bgRender.material.SetTextureOffset("_MainTex", offsetVector);
    }

    private void ResetTimer()
    {
        timeLeft = 60f;
    }

    private void UpdateTimer(float deltaTime)
    {
        timeLeft -= deltaTime;
        timerText.text = "Time Left: " + timeLeft.ToString("F1");
    }
}
