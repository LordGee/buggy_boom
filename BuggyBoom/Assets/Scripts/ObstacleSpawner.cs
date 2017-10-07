using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

    // Public Variables
    [Tooltip("Attach Prefab of cube with appropriate Rigidbody and COnstraints")] public GameObject carObstacle;

    [Tooltip("Define how often a Wall is Spawned")] public float spawnInterval = 1f;
    [Tooltip("Define time until the first object is spawned")] public float timeBeforeFirstSpawn = 15f;
    public int[] laneArray = {-3, -1, 1, 3};

    // Private Variables
    private float timeSinceLastSpawn = 0f;
    private bool startSpawning = false;


    // Update is called once per frame
    void Update()
    {
        if (!startSpawning && Time.timeSinceLevelLoad > timeBeforeFirstSpawn)
        {
            startSpawning = true;
        }
        if (startSpawning && Time.timeSinceLevelLoad - timeSinceLastSpawn > spawnInterval)
        {
            GameObject newObstacle = Instantiate(
                carObstacle, 
                new Vector3(
                    laneArray[Random.Range(0, laneArray.Length)], 
                    transform.position.y, 
                    transform.position.z), 
                Quaternion.identity);
            newObstacle.transform.parent = gameObject.transform;
            timeSinceLastSpawn = Time.timeSinceLevelLoad;
        }
    }
}
