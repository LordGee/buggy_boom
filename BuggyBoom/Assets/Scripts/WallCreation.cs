using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCreation : MonoBehaviour {

    public float wallSpeed = 5f;

    private float scaleX, scaleY, scaleZ;
    private Color col;

    // Use this for initialization
    void Start () {
        GetRandomSize();
        transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0f, 0f, -(wallSpeed * Time.deltaTime));
	}

    private void GetRandomSize()
    {
        scaleX = Random.Range(2f, 8f);
        scaleY = Random.Range(2f, 10f);
        scaleZ = Random.Range(2f, 5f);
    }
}
