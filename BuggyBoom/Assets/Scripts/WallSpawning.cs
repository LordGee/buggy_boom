using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawning : MonoBehaviour
{

    public GameObject wall;
    public float timeSinceLastSpawn = 0f;
    public float spawnInterval = 1f;
	
	// Update is called once per frame
	void Update () {
	    if (Time.timeSinceLevelLoad - timeSinceLastSpawn > spawnInterval)
	    {
	        Vector3 pos = transform.position;
	        GameObject newWall = Instantiate(wall, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);
	        newWall.transform.parent = gameObject.transform;
	        timeSinceLastSpawn = Time.timeSinceLevelLoad;
	    }
	}
}
