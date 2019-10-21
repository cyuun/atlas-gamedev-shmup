using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")] public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10f;
    public int score = 100;

    [Header("Set Dynamically")] public int identifierNum;

    protected BoundsCheck bndCheck;
    private Renderer wingRender;

    public Vector3 pos
    {
        get { return (this.transform.position); }
        set { this.transform.position = value; }
    }

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        wingRender = transform.Find("Wing").GetComponent<Renderer>();
        identifierNum = Random.Range(0, 3);
        switch (identifierNum)
        {
            case 0:
                wingRender.material = Main.S.materials[0];
                break;
            case 1:
                wingRender.material = Main.S.materials[1];
                break;
            case 2:
                wingRender.material = Main.S.materials[2];
                break;
        }
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
        tempPos.x -= speed * Time.deltaTime;
        pos = tempPos;
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject otherGo = other.gameObject;
        if (otherGo.tag == "ProjectileHero")
        {
            if (otherGo.GetComponent<Projectile>() != null)
            {
                if (otherGo.GetComponent<Projectile>().identifierNum == identifierNum)
                {
                    UIDaddy.S.score = UIDaddy.S.score + 1;
                }
                else
                {
                    UIDaddy.S.IncrementWrongEnemy(otherGo.GetComponent<Projectile>().identifierNum);
                }
            }
            Destroy(otherGo);
            Destroy(gameObject);
        }
        else
        {
            print("Enemy hit by non-ProjectileHero: " + otherGo.name);
        }
    }
}