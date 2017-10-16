﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

    // TODO: Make the spawn interval dynamic, interval gradullay decreases.
    // TODO: Add additional obstacles, such as barriers and pedestrians.
    // TODO: Add boss vehcle to spawn after set time of play.
    // TODO: Add several enum states to define appropriate obstacles and duration.

    // Public Variables
    [Tooltip("Define how often an obstacle is Spawned (Min / Max)")]
    public float spawnMin, spawnMax;
    
    [Tooltip("Define time until the first object is spawned")]
    public float timeBeforeFirstSpawn = 5f;

    // Private Variables
    private float timeSinceLastSpawn = 0f;
    private bool startSpawning = false;
    private float spawnInterval;
    
    private int[] laneArray;
    private GameObject obstacle;
    private GameControlScript gameControl;

    private int roadBlockCount = 3;

    void Start()
    {
        gameControl = FindObjectOfType<GameControlScript>();
    }

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
            obstacle = gameControl.GetNpcGameObjectToSpawn();
            laneArray = gameControl.GetLaneArray();
            if (obstacle.tag == "RoadBlock")
            {
                RoadBlockSpawn();
            }
            else
            {
                Spawn();
            }
            

            timeSinceLastSpawn = Time.timeSinceLevelLoad;
            
        }
    }

    void Spawn()
    {
        GameObject newObstacle = Instantiate(obstacle, new Vector3(laneArray[Random.Range(0, laneArray.Length)], transform.position.y, transform.position.z), Quaternion.identity);
        newObstacle.transform.parent = gameObject.transform;
        spawnInterval = Random.Range(spawnMin, spawnMax);
    }

    void RoadBlockSpawn()
    {
        int[] tempLanes = new int[roadBlockCount];
        int insertCount = roadBlockCount;
        bool test = true;
        for (int i = 0; i < tempLanes.Length; i++)
        {
            tempLanes[i] = Random.Range(0, laneArray.Length);
            for (int j = 0; j < tempLanes.Length; j++)
            {
                if (tempLanes[i] == tempLanes[j] && i != j)
                {
                    test = false;
                }
            }
            if (!test)
            {
                i--;
                test = true;
            }
        }
        for (int i = 0; i < tempLanes.Length; i++)
        {
            GameObject newObstacle = Instantiate(obstacle, new Vector3(laneArray[tempLanes[i]], transform.position.y, transform.position.z), Quaternion.identity);
            newObstacle.transform.parent = gameObject.transform;
            spawnInterval = Random.Range(spawnMin + 0.5f, spawnMax + 0.5f);
        }
    }
}
