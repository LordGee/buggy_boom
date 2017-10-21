using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawning : MonoBehaviour
{
    // Public Variables
    [Tooltip("Attach Prefab of cube with appropriate Rigidbody and Constraints")]
    public GameObject wall;
    
    // Private Variables
    private float timeBeforeFirstSpawn = 2f;
    private float spawnInterval = 0.5f;
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
