using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDaddy : MonoBehaviour
{
    public static UIDaddy S;
    
    [Header("Set in Inspector")]
    public int maxWrongEnemies = 5;
    public GameObject wrongEnemyPrefab;

    [Header("Set Dynamically")]
    public Text scoreText;
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
    void Awake()
    {
        S = this;
        scoreText = transform.Find("Canvas/Score").gameObject.GetComponent<Text>();
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
}
