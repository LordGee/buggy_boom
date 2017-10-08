using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public Variables
    [Tooltip("The spped at which the Buggy will translate when input is detected")]
    public float buggySpeed = 10f;

    public float buggySpeedRot = 50f;


    private float StartZ;


    void Start()
    {
        StartZ = transform.position.z;
    }

	// Update is called once per frame
	void Update ()
	{
	    transform.Translate(GetTranslatedPosition(buggySpeed), 0f, 0f);
	    if (Input.GetAxis("Horizontal") !=0)
	    {
	        transform.Rotate(0f, GetTranslatedPosition(buggySpeedRot), 0f);
        }
	    else
	    {
            transform.Rotate(0f, GetTranslatedPosition(transform.rotation.y * -1, buggySpeedRot * 20f), 0f);
        }
	    

        // Checks commonly horizontal type movements
        
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

	    if (transform.position.z < StartZ)
	    {
	        transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                StartZ);
	    }
	}

    private float GetTranslatedPosition(float _speed)
    {
        return Input.GetAxis("Horizontal") * _speed * Time.deltaTime;
    }

    private float GetTranslatedPosition(float _speed, float _move)
    {
        return _move * _speed * Time.deltaTime;
    }
}
