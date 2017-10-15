using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObstacle : MonoBehaviour {

    //Public Variables
    
    [Tooltip("Gameobject instantiated and provides effects when the object collides with the player")]
    public GameObject DeathParticleEffect;

    // Private Variables
    private GameControlScript gameControl;
    private float npcHealth;
    private float npcDamage;
    private float npcSpeed;

    void Start()
    {
        gameControl = FindObjectOfType<GameControlScript>();
        npcHealth = gameControl.GetNpcHealth(gameObject);
        npcDamage = gameControl.GetNpcDamage(gameObject);
        npcSpeed = gameControl.GetNpcSpeed(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0f, 0f, -(npcSpeed * Time.deltaTime));
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            NpcDeathEffect();
            gameControl.DamagePlayer(npcDamage);
            gameControl.AddPoints(npcSpeed);
            Destroy(this.gameObject, 0.1f); // added this small amount of time makes such a difference
        }
        else if (col.gameObject.tag == "Projectile")
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
