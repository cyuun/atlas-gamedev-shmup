using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float       speed            =  30;
    public float       rollMult         = -45;
    public float       pitchMult        =  30;
    public float       gameRestartDelay =  2f;
    public GameObject  projectilePrefab;
    public float       projectileSpeed  =  40;
    public KeyCode     upKey;
    public KeyCode     downKey;
    public KeyCode     fireKey;
    public int         identifierNum = 0;

    [Header("Set Dynamically")]
    [SerializeField]
    private float       _shieldLevel    =   1;

    private GameObject lastTriggerGo    = null;
    private GameObject shieldGo;
    private GameObject wingGo;
    private GameObject cockCubeGo;
    private GameObject weaponGo;
    private Shield     shield;
    private Renderer   wingRender;
    private Renderer   cockCubeRender;
    private Weapon     weapon;

    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;

    private void Awake()
    {
        shieldGo = transform.Find("Shield").gameObject;
        wingGo = transform.Find("Wing").gameObject;
        cockCubeGo = transform.Find("Cockpit/Cube").gameObject;
        weaponGo = transform.Find("Weapon").gameObject;
        shield = shieldGo.GetComponent<Shield>();
        wingRender = wingGo.GetComponent<Renderer>();
        cockCubeRender = cockCubeGo.GetComponent<Renderer>();
        weapon = weaponGo.GetComponent<Weapon>();
        switch (identifierNum)
        {
            case 1:
                wingRender.material = Main.S.materials[1];
                cockCubeRender.material = Main.S.materials[1];
                break;
            case 2:
                wingRender.material = Main.S.materials[2];
                cockCubeRender.material = Main.S.materials[2];
                break;
            default:
                wingRender.material = Main.S.materials[0];
                cockCubeRender.material = Main.S.materials[0];
                break;
        }
    }

    void Update()
    {
        float yAxis = 0f;
        if (Input.GetKey(upKey))
        {
            yAxis += 1f;
        }

        if (Input.GetKey(downKey))
        {
            yAxis -= 1f;
        }

        Vector3 pos        = transform.position;
        pos.y             += yAxis * speed * Time.deltaTime;
        transform.position = pos;
        
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, 0, 0);

        if (Input.GetKeyDown(fireKey) && fireDelegate != null)
        {
            fireDelegate();
        }
    }

    private void TempFire()
    {
        GameObject projGo = Instantiate<GameObject>(projectilePrefab);
        projGo.transform.position = transform.position;
        Rigidbody rb = projGo.GetComponent<Rigidbody>();

        Projectile proj = projGo.GetComponent<Projectile>();
        proj.type = WeaponType.blaster;
        float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
        rb.velocity = Vector3.right * tSpeed;
    }

    public void AbsorbPowerUp(GameObject go)
    {
        Powerup pu = go.GetComponent<Powerup>();
        switch (pu.type)
        {
            case WeaponType.shield:
                shieldLevel++;
                shield.changeShieldLevel(shieldLevel);
                break;
            default:
                weapon.SetType(pu.type);
                break;
        }
        pu.AbsorbedBy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        if (go == lastTriggerGo) return;
        lastTriggerGo = go;

        if (go.tag == "Enemy")
        {
            shieldLevel--;
            shield.changeShieldLevel(shieldLevel);
            Destroy(go);
        }
        else if (go.tag == "PowerUp")
        {
            AbsorbPowerUp(go);
        }
        else
        {
            print("Triggered by non-Enemy: " + go.name);
        }
    }

    public float shieldLevel
    {
        get { return (_shieldLevel); }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            if (value < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }
}
