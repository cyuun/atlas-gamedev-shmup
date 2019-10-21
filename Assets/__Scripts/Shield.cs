using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float     rotationsPerSecond = 0.1f;

    [Header("Set Dynamically")] [SerializeField]
    private int      _level;
    public float     shieldLevel
    {
        get { return (float)_level; }
        set
        {
            _level = Mathf.FloorToInt(value);
            mat.mainTextureOffset = new Vector2(0.2f * _level, 0);
        }
    }

    private Material mat;
    
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        shieldLevel = 1f;
    }

    void Update()
    {
        float rZ           = -(rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(x: 0, y: 0, z: rZ);
    }
}
