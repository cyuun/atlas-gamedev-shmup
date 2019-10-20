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
    public GameObject   prefabHero;
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

        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
        
        SpawnHeros();
    }

    public void StartPlaying()
    {
       Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
       Invoke(methodName: "SpawnPowerup", time:1f / powerupSpawnPerSecond); 
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
        Invoke("SoftRestart", delay);
    }

    public void Restart()
    {
        SceneManager.LoadScene("_Scene_0");
    }

    public void SoftRestart()
    {
        if (Hero.S0 != null)
        {
            Hero.S0.Despawn();
        }
        if (Hero.S1 != null)
        {
            Hero.S1.Despawn();
        }
        if (Hero.S2 != null)
        {
            Hero.S2.Despawn();
        }
        UIDaddy.S.score = 0;
        SpawnHeros();
    }

    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if (WEAP_DICT.ContainsKey(wt))
        {
            return WEAP_DICT[wt];
        }
        return new WeaponDefinition();
    }

    public void SpawnHeros()
    {
        GameObject h0 = Instantiate<GameObject>(prefabHero);
        if (h0.GetComponent<Hero>() != null)
        {
            h0.GetComponent<Hero>().identifierNum = 0;
        }
        h0.transform.position = new Vector3(-50, 25, 0);

        GameObject h1 = Instantiate<GameObject>(prefabHero);
        if (h1.GetComponent<Hero>() != null)
        {
            h1.GetComponent<Hero>().identifierNum = 1;
        }
        h1.transform.position = new Vector3(-50, 0, 0);

        GameObject h2 = Instantiate<GameObject>(prefabHero);
        if (h2.GetComponent<Hero>() != null)
        {
            h2.GetComponent<Hero>().identifierNum = 2;
        }
        h2.transform.position = new Vector3(-50, -25, 0);
    }
}
