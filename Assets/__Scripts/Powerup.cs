using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2( .25f, 2);
    public float   lifeTime = 6f;
    public float   fadeTime = 4f;


    [Header("Set Dynamically")]
    public WeaponType type;
    public GameObject cube;
    public TextMesh letter;
    public Vector3 rotPerSecond;
    public float birthTime;
    
    private Rigidbody rb;
    private BoundsCheck bc;
    private Renderer cubeRender;
    
    void Awake()
    {
        cube = transform.Find("Cube").gameObject;
        letter = GetComponent<TextMesh>();
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoundsCheck>();
        cubeRender = cube.GetComponent<Renderer>();

        Vector3 vel = Random.onUnitSphere;
        vel.z = 0;
        vel.Normalize();

        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rb.velocity = vel;
    }

    void Update()
    {
        
    }
}
