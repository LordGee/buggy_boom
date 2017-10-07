using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObstacle : MonoBehaviour {

    //Public Variables
    public float obstacleCurrentSpeed = 10f;  

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0f, 0f, -(obstacleCurrentSpeed * Time.deltaTime));
    }
}
