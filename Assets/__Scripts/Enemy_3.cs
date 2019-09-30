using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy
{
    [Header("Set in Inspector: Enemy_3")]
    public float lifeTime = 5;

    [Header("Set Dynamically: Enemy_3")]
    public Vector3[] points;
    public float birthTime;
    void Start()
    {
        points = new Vector3[3];
        points[0] = pos;

        float yMin = -bndCheck.camHeight + bndCheck.radius;
        float yMax =  bndCheck.camHeight - bndCheck.radius;

        Vector3 v = Vector3.zero;
        v.x = -bndCheck.camWidth * UnityEngine.Random.Range(2.75f, 2);
        v.y = UnityEngine.Random.Range(yMin, yMax);
        points[1] = v;
        
        v = Vector3.zero;
        v.x = pos.x;
        v.y = UnityEngine.Random.Range(yMin, yMax);
        points[2] = v;

        birthTime = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;
        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        Vector3 p01, p12;
        u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
        p01 = (1 - u) * points[0] + u * points[1];
        p12 = (1 - u) * points[1] + u * points[2];
        pos = (1 - u) * p01       + u * p12;
    }
}
