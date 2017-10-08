using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObstacle : MonoBehaviour {

    //Public Variables
    [Tooltip("The speed of the obstacle, moving towards the player")]
    public float obstacleCurrentSpeed = 10f;
    [Tooltip("Gameobject instantiated and provides effects when the object collides with the player")]
    public GameObject DeathParticleEffect;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0f, 0f, -(obstacleCurrentSpeed * Time.deltaTime));
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameObject death = Instantiate(DeathParticleEffect, transform.position, Quaternion.identity);
            Destroy(death, 2f);
            Destroy(this.gameObject, 0.1f); // added this small amount of time makes such a difference
        }
    }


}
