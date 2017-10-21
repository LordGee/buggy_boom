using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeepNPC : MonoBehaviour {

    // Public Variables
    [Tooltip("Two Materials to define type of jeep")]
    public Material[] materials;
    [Tooltip("Projectile that the jeep shoots")]
    public GameObject projectile;

    // Private Variables
    private GameControlScript gameControl;
    private new AudioSource audio;
    private int[] roadLaneArray = { -3, -1, 1, 3 };
    private bool shooter;
    private float shootTimer = 0f, shootFreq = 1.5f, minShoot = 0.5f, maxShoot = 2.5f;
    private float projectileLife = 4f;
    private float npcDamage, npcHealth;
    


    // Use this for initialization
    void Start()
    {
        gameControl = FindObjectOfType<GameControlScript>();
        npcDamage = gameControl.GetNpcDamage();
        npcHealth = gameControl.GetNpcHealth();
        IsShooterAndMaterial();
        DestroyGlitchedObject();
    }

    /* Randomly chooses if next spawn Jeep is a shooter this then 
     * determines which material to provide. */
    void IsShooterAndMaterial()
    {
        GameObject obj = transform.Find("JEEP_BODY").gameObject;
        if (Random.Range(0, 3) == 1)
        {
            shooter = true;
            obj.GetComponent<Renderer>().material = materials[1];
            audio = GetComponent<AudioSource>();
            shootTimer = Time.timeSinceLevelLoad;
        }
        else
        {
            shooter = false;
            obj.GetComponent<Renderer>().material = materials[0];
        }
    }

    void Update()
    {
        if (shooter && Time.timeSinceLevelLoad - shootTimer > shootFreq)
        {
            ShootProjectile();
            audio.Play();
            shootTimer = Time.timeSinceLevelLoad;
            shootFreq = Random.Range(minShoot, maxShoot);
        }
    }

    /* Projectile is instantiated and the unique damage for the object
     * is passed to the projectile script */
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

    /* After a boss and before road blocks a glitched Jeep is spawned with is 
     * super doper powerful, it has the same attributes as a road block so if 
     * crashed into or hits with projectile will instantly kill the player. 
     * This is a tempory fix. */
    private void DestroyGlitchedObject()
    {
        if (npcHealth > 10000)
        {
            Destroy(gameObject);
        }
    }
}
