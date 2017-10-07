using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawning : MonoBehaviour
{
    // Public Variables
    [Tooltip("Attach Prefab of cube with appropriate Rigidbody and COnstraints")]
    public GameObject wall;
    [Tooltip("Define how often a Wall is Spawned")]
    public float spawnInterval = 0.5f;
    [Tooltip("Define time until the first object is spawned")]
    public float timeBeforeFirstSpawn = 10f;

    // Private Variables
    private float timeSinceLastSpawn = 0f;

    private bool startSpawning = false;
    

    // Update is called once per frame
    void Update () {
        if (!startSpawning && Time.timeSinceLevelLoad > timeBeforeFirstSpawn)
        {
            startSpawning = true;
        }
	    if (startSpawning && Time.timeSinceLevelLoad - timeSinceLastSpawn > spawnInterval)
	    {
	        Vector3 pos = transform.position;
	        GameObject newWall = Instantiate(wall, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);
	        newWall.transform.parent = gameObject.transform;
	        timeSinceLastSpawn = Time.timeSinceLevelLoad;
	    }
	}
}
