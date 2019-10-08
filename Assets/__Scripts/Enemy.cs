using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed    =  10f;
    public float fireRate = 0.3f;
    public float health   =  10f;
    public int   score    =  100;

    [Header("Set Dynamically")]
    public int identifierNum;

    protected BoundsCheck bndCheck;

    public Vector3 pos
    {
        get { return (this.transform.position); }
        set { this.transform.position = value; }
    }

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    void Update()
    {
        Move();
        if (bndCheck != null && (bndCheck.offDown || bndCheck.offUp || bndCheck.offLeft))
        {
            Destroy(gameObject);
        }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.x      -= speed * Time.deltaTime;
        pos             = tempPos;
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject otherGo = other.gameObject;
        if (otherGo.tag == "ProjectileHero")
        {
            Destroy(otherGo);
            Destroy(gameObject);
        }
        else
        {
            print("Enemy hit by non-ProjectileHero: " + otherGo.name);
        }
    }
}
