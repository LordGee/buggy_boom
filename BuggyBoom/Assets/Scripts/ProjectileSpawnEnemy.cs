using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawnEnemy : MonoBehaviour {

    public float projectileSpeed = 24f;
    private GameObject player;
    private Vector3 originalPlayerPosision;
    public float projectileDamage = 10f;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        originalPlayerPosision = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 1);
    }

    // Update is called once per frame
    void Update()
    {
        // https://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html
        transform.position = Vector3.MoveTowards(
            transform.position,
            originalPlayerPosision,
            projectileSpeed * Time.deltaTime);
        if (transform.position == originalPlayerPosision)
        {
            Destroy(gameObject);
        }
    }

    public float GetProjectileDamage()
    {
        return projectileDamage;
    }
}
