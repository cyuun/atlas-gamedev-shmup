using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Main : MonoBehaviour
{
    public  static Main S;
    private static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public GameObject   powerupPrefab;
    public float        enemySpawnPerSecond = 0.5f;
    public float        enemyDefaultPadding = 1.5f;
    public WeaponDefinition[] weaponDefinitions;
    public WeaponType[] powerupTypes = new WeaponType[]
    {
        WeaponType.blaster,
        WeaponType.spread,
        WeaponType.shield
    };
    public Material[]   materials;
    public Vector3[,]   wrongEnemyCapsulePositions = new Vector3[3,5]
    {
        {new Vector3(-30,-35,0), new Vector3(-28,-35,0), new Vector3(-26,-35,0), new Vector3(-24,-35,0), new Vector3(-22,-35,0)},
        {new Vector3(-10,-35,0), new Vector3(-8,-35,0), new Vector3(-6,-35,0), new Vector3(-4,-35,0), new Vector3(-2,-35,0)},
        {new Vector3(10,-35,0), new Vector3(12,-35,0), new Vector3(14,-35,0), new Vector3(16,-35,0), new Vector3(18,-35,0)}
    };
    public KeyCode[,]   heroKeySets = new KeyCode[3,3]
    {
        {KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow},
        {KeyCode.P, KeyCode.Semicolon, KeyCode.Quote},
        {KeyCode.W, KeyCode.S, KeyCode.D}
    };
    public float        powerupSpawnPerSecond = 0.1f;
    public float        powerupDefaultPadding = 1;

    private BoundsCheck bndCheck;

    private void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();

        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
        Invoke(methodName: "SpawnPowerup", time:1f / powerupSpawnPerSecond);

        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }

    public void SpawnPowerup()
    {
        GameObject go = Instantiate<GameObject>(powerupPrefab);

        float powerupPadding = powerupDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            powerupPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        
        Vector3 pos = Vector3.zero;
        float yMin = -bndCheck.camHeight + powerupPadding;
        float yMax =  bndCheck.camHeight - powerupPadding;
        pos.y = Random.Range(yMin, yMax);
        pos.x = bndCheck.camWidth - powerupPadding;
        go.transform.position = pos;

        int ndx = Random.Range(0, powerupTypes.Length);
        WeaponType puType = powerupTypes[ndx];
        Powerup pu = go.GetComponent<Powerup>();
        pu.SetType(puType);

        Invoke(methodName: "SpawnPowerup", time: 1f / powerupSpawnPerSecond);
    }

    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        
        Vector3 pos = Vector3.zero;
        float yMin = -bndCheck.camHeight + enemyPadding;
        float yMax =  bndCheck.camHeight - enemyPadding;
        pos.y = Random.Range(yMin, yMax);
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

    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if (WEAP_DICT.ContainsKey(wt))
        {
            return WEAP_DICT[wt];
        }
        return new WeaponDefinition();
    }
}
