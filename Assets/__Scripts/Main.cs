using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class Main : MonoBehaviour
{
    static public Main S;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float        enemySpawnPerSecond = 0.5f;
    public float        enemyDefaultPadding = 1.5f;

    private BoundsCheck bndCheck;

    private void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        
        Invoke("SpawnEnemy", 1f/enemySpawnPerSecond);
    }

    public void SpawnEnemy()
    {
        int ndx = UnityEngine.Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        
        Vector3 pos = Vector3.zero;
        float yMin = -bndCheck.camHeight + enemyPadding;
        float yMax =  bndCheck.camHeight - enemyPadding;
        pos.y = UnityEngine.Random.Range(yMin, yMax);
        pos.x = bndCheck.camWidth + enemyPadding;
        go.transform.position = pos;
        
        Invoke("SpawnEnemy", 1f/enemySpawnPerSecond);
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        SceneManager.LoadScene("_Scene_0");
    }
}
