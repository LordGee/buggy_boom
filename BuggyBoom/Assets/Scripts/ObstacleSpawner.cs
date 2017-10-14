using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

    // TODO: Make the spawn interval dynamic, interval gradullay decreases.
    // TODO: Add additional obstacles, such as barriers and pedestrians.
    // TODO: Add boss vehcle to spawn after set time of play.
    // TODO: Add several enum states to define appropriate obstacles and duration.

    // Public Variables
    [Tooltip("Attach Prefab of cube with appropriate Rigidbody and COnstraints")]
    public GameObject carObstacle;
    [Tooltip("Define how often a Wall is Spawned")]
    public float spawnInterval = 0.8f;
    [Tooltip("Define time until the first object is spawned")]
    public float timeBeforeFirstSpawn = 5f;

    public Material[] materials;


    // Private Variables
    private float timeSinceLastSpawn = 0f;
    private bool startSpawning = false;

    public int[] laneArray = { -3, -1, 1, 3 };



    // Update is called once per frame
    void Update()
    {
        // checks to see if enough time has elapsed before starting the spawn process
        if (!startSpawning && Time.timeSinceLevelLoad > timeBeforeFirstSpawn)
        {
            startSpawning = true;
        }
        /*  once the spawn process has starteded a new a new car object will be spawned at
            specific intervals within the bounds af a random lane on the grid. The object 
            will be child of the parent spawner and the time counter since the last spawn 
            is then reset   */
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
            GameObject obj = newObstacle.transform.Find("JEEP_BODY").gameObject;
            
            obj.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
            timeSinceLastSpawn = Time.timeSinceLevelLoad;
        }
    }
}
