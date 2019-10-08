using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float     rotationsPerSecond = 0.1f;

    [Header("Set Dynamically")]
    public int       levelShown         =    0;

    private Material mat;
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        float rZ           = -(rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(x: 0, y: 0, z: rZ);
    }

    public void changeShieldLevel(float level)
    {
        levelShown = Mathf.FloorToInt(level);
        mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
    }
}
