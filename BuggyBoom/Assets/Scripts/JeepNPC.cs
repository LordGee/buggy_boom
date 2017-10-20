using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeepNPC : MonoBehaviour {

    public Material[] materials;
    public GameObject projectile;
    private GameControlScript gameControl;
    private int[] roadLaneArray = { -3, -1, 1, 3 };
    private bool shooter = false;
    private float shootTimer;
    private float shootFreq = 3f;
    private float projectileLife = 4f;
    private float npcDamage;


    // Use this for initialization
    void Start()
    {
        gameControl = FindObjectOfType<GameControlScript>();
        npcDamage = gameControl.GetNpcDamage();
        GameObject obj = transform.Find("JEEP_BODY").gameObject;
        if (Random.Range(0,3) == 1)
        {
            shooter = true;
            obj.GetComponent<Renderer>().material = materials[1];
            shootTimer = Time.timeSinceLevelLoad;
        }
        else
        {
            obj.GetComponent<Renderer>().material = materials[0];
        }
    }

    void Update()
    {
        if (shooter && Time.timeSinceLevelLoad - shootTimer > shootFreq)
        {
            ShootProjectile();
            shootTimer = Time.timeSinceLevelLoad;
        }
    }

    void ShootProjectile()
    {
        GameObject proj = Instantiate(projectile,
                new Vector3(transform.position.x, transform.position.y, transform.position.z - 1),
                Quaternion.identity);
        proj.GetComponent<ProjectileSpawnEnemy>().projectileDamage = npcDamage;
        Destroy(proj, projectileLife);
    }

    public int[] GetLaneArray()
    {
        return roadLaneArray;
    }
}
