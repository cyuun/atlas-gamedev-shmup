using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is an enum of the various possible weapon types.
/// It also includes a "shield" type to allow a shield power-up.
/// Items marked [NI] below are Not Implemented in the IGPD book.
/// </summary>

public enum WeaponType
{
    none,
    blaster,
    spread,
    shield
}

/// <summary>
/// The WeaponDefinition class allows you to set the properties
///  of a specific weapon in the Inspector. The Main class has
///  an array of WeaponDefinitions that makes this possible.
/// </summary>

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string     letter;
    public Color      color = Color.white;
    public GameObject projectilePrefab;
    public Color      projectileColor = Color.white;
    public float      damageOnHit = 0;
    public float      continuousDamage = 0;
    public float      delayBetweenShots = 0;
    public float      velocity = 20;
}

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dynamically")] [SerializeField]
    private WeaponType _type;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime;

    private Renderer collarRend;

    public WeaponType type
    {
        get { return _type; }
        set { SetType(value); }
    }

    public void SetType(WeaponType wt)
    {
        _type = wt;
        if (type == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefinition(_type);
        collarRend.material.color = def.color;
        lastShotTime = 0;
    }

    void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();

        SetType(_type);

        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        GameObject rootGo = transform.root.gameObject;
        if (rootGo.GetComponent<Hero>() != null)
        {
            rootGo.GetComponent<Hero>().fireDelegate += Fire;
        }
    }

    public void Fire()
    {
        if (gameObject.activeInHierarchy == false || Time.time - lastShotTime < def.delayBetweenShots)
        {
            return;
        }
        Projectile p;
        Vector3 vel = Vector3.right * def.velocity;
        if (transform.right.x < 0)
        {
            vel.x = -vel.x;
        }
        switch (type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.rb.velocity = vel;
                break;
            case WeaponType.spread:
                p = MakeProjectile();
                p.rb.velocity = vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rb.velocity = p.transform.rotation * vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rb.velocity = p.transform.rotation * vel;
                break;
        }
    }

    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }

        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);

        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time;
        return p;
    }
}
