using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNPC : MonoBehaviour {

    //Public Variables

    [Tooltip("Gameobject instantiated and provides effects when the object collides with the player")]
    public GameObject DeathParticleEffect;

    // Private Variables
    private GameControlScript gameControl;
    private float npcHealth;
    private float npcDamage;
    private float npcSpeed;
    private bool direction = true;

    public GameObject projectile;
    private float shootTimer;
    private float shootFreq = 1f;
    private float projectileLife = 4f;

    void Start()
    {
        gameControl = FindObjectOfType<GameControlScript>();
        npcHealth = gameControl.GetNpcHealth();
        npcDamage = gameControl.GetNpcDamage();
        npcSpeed = gameControl.GetNpcSpeed();
        // transform.rotation.y.Equals(90f);
    }

    // Update is called once per frame
    void Update()
    {
        float speed = npcSpeed * Time.deltaTime;
        bool move = false;
        if (transform.position.z > 3f)
        {
            transform.Translate(0f, 0f, -speed);
            move = false;
        }
        else
        {
            move = true;
        }
        if (move)
        {
            if (direction)
            {
                transform.Translate(speed, 0f, 0f);
                if (transform.position.x > 4.0f)
                {
                    direction = false;
                }
            }
            else
            {
                transform.Translate(-speed, 0f, 0f);
                if (transform.position.x < -4)
                {
                    direction = !direction;
                }
            }
        }
        if (Time.timeSinceLevelLoad - shootTimer > shootFreq)
        {
            GameObject proj = Instantiate(projectile,
                new Vector3(transform.position.x, transform.position.y, transform.position.z - 1),
                Quaternion.identity);
            proj.GetComponent<ProjectileSpawnEnemy>().projectileDamage = npcDamage;
            Destroy(proj, projectileLife);
            shootTimer = Time.timeSinceLevelLoad;
            shootFreq = Random.Range(0f, 1f);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Projectile")
        {
            gameControl.DamageNPC(this.gameObject, gameControl.GetPlayerPoints(), gameControl.GetPlayerDamage(), ref npcHealth);
            if (npcHealth <= 0)
            {
                NpcDeathEffect();
            }
            Destroy(col.transform.parent.gameObject);
        }
    }

    void NpcDeathEffect()
    {
        GameObject death = Instantiate(DeathParticleEffect, transform.position, Quaternion.identity);
        Destroy(death, 3f);
    }
}
