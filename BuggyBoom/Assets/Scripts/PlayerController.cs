using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public Variables
    [Tooltip("The spped at which the Buggy will translate when input is detected")]
    public float buggySpeed = 10f;
    
	
	// Update is called once per frame
	void Update ()
	{
        // Checks commonly horizontal type movements
	    float trans = Input.GetAxis("Horizontal") * buggySpeed * Time.deltaTime;
	    transform.Translate(trans, 0f, 0f);
        if (transform.position.x > 5f)
        {
            transform.position = new Vector3(5f, 
                transform.position.y, 
                transform.position.z);
        }
        else if (transform.position.x < -5f)
        {
            transform.position = new Vector3(-5f,
                transform.position.y,
                transform.position.z);
        }
	}
}
