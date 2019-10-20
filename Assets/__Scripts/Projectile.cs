using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck bndCheck;

    [Header("Set Dynamically")]
    public Rigidbody rb;
    public int identifierNum;
    public MeshRenderer mRend;
    public TrailRenderer tRend;

    [SerializeField]
    private WeaponType _type;

    public WeaponType type
    {
        get { return _type; }
        set { SetType(value); }
    }

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        mRend = GetComponent<MeshRenderer>();
        tRend = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (bndCheck.offRight)
        {
            Destroy(gameObject);
        }
    }

    public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
    }
}
