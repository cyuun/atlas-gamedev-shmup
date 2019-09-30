using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    [Header("set in Inspector")]
    public float waveFrequency =  2;
    public float waveWidth     =  4;
    public float waveRotX      = 45;

    private float y0;
    private float birthTime;
    void Start()
    {
        y0 = pos.y;
        birthTime = Time.time;
    }

    public override void Move()
    {
        Vector3 tempPos = pos;
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.y = y0 + waveWidth * sin;
        pos = tempPos;
        
        Vector3 rot = new Vector3(sin * waveRotX, 0, 0);
        this.transform.rotation = Quaternion.Euler(rot);

        base.Move();
    }
}
