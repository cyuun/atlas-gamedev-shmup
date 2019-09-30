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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
