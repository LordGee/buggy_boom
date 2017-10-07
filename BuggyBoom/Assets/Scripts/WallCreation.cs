using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCreation : MonoBehaviour
{
    //Public Variables
    public float wallCurrentSpeed = 10f;
    

    // Private Variables
    private float scaleX, scaleY, scaleZ;
    private Color col;

    // Use this for initialization
    void Start () {
        GetRandomSize();
        transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

        // Reference: https://docs.unity3d.com/ScriptReference/Random.ColorHSV.html 
        GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0f, 0f, -(wallCurrentSpeed * Time.deltaTime));
	}

    private void GetRandomSize()
    {
        scaleX = Random.Range(2f, 8f);
        scaleY = Random.Range(2f, 10f);
        scaleZ = Random.Range(2f, 5f);
    }
}
