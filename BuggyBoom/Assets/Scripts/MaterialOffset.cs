﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialOffset : MonoBehaviour
{
    // Public Variables
    [Tooltip("Define the starting speed of the material")]
    public float offsetSpeed = 0.1f;
    [Tooltip("Define the theshold of the acceleration period")]
    public float initialThreshold = 2.9f;
    [Tooltip("Define the acceleration rate prior to reaching threshold")]
    public float preIncrementSpeed = 0.001f;

    // Private Variables
    private Renderer rend;

	// Use this for initialization
	void Start ()
	{
	    rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		rend.material.SetTextureOffset("_MainTex", new Vector2(0f, -(Time.time * offsetSpeed)));
	    if (offsetSpeed < initialThreshold)
	    {
	        offsetSpeed += preIncrementSpeed;
	    }
	}

    
}
