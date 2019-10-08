using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 speedMinMax = new Vector2(2.5f, 30);
    public float   lifeTime = 18f;
    public float   fadeTime = 16f;


    [Header("Set Dynamically")]
    public WeaponType type;
    public GameObject cube;
    public TextMesh letter;
    public Vector3 rotPerSecond;
    public float birthTime;
    public float speed;
    
    private Rigidbody rb;
    private BoundsCheck bc;
    private Renderer cubeRender;

    public Vector3 pos
    {
        get { return this.transform.position; }
        set { this.transform.position = value; }
    }
    
    void Awake()
    {
        cube = transform.Find("Cube").gameObject;
        letter = GetComponent<TextMesh>();
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoundsCheck>();
        cubeRender = cube.GetComponent<Renderer>();

        speed = Random.Range(speedMinMax.x, speedMinMax.y);
        Vector3 vel = Vector3.left * speed;
        rb.velocity = vel;

        transform.rotation = Quaternion.identity;
        
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y));
        birthTime = Time.time;

        int typeNum = Random.Range(1, 4);
    }

    void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        float u = (Time.time - birthTime - lifeTime) / fadeTime;
        if (u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }

        if (u > 0)
        {
            Color c = cubeRender.material.color;
            c.a = 1f - u;
            cubeRender.material.color = c;

            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

        if (bc.isOnScreen == false)
        {
            Destroy(gameObject);
        }
        
    }

    public void SetType(WeaponType wt)
    {
        WeaponDefinition def = Main.GetWeaponDefinition(wt);
        cubeRender.material.color = def.color;
        letter.text = def.letter;
        type = wt;
    }

    public void AbsorbedBy(GameObject target)
    {
        Destroy(this.gameObject);
    }
}
