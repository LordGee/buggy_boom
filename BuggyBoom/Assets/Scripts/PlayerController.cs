using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public Variables
    [Tooltip("The spped at which the Buggy will translate when input is detected")]
    public float buggySpeed = 10f;
    public float buggySpeedRot = 50f;
    public GameObject projectile;
    public float projectileRate = 0.2f;

    // Private Variables
    private float StartZ;
    private float projectileCooldown;


    void Start()
    {
        StartZ = transform.position.z;
        projectileCooldown = Time.timeSinceLevelLoad;
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

	    if (Input.GetAxis("Jump") != 0 || Input.GetAxis("Fire1") != 0)
	    {
	        if (Time.timeSinceLevelLoad - projectileCooldown > projectileRate)
	        {
	            Instantiate(projectile, transform.position, Quaternion.identity);
	            projectileCooldown = Time.timeSinceLevelLoad;
	        }
	    }
	    
        CheckPositioningConstraints();
        
	}

    private float GetTranslatedPosition(float _speed)
    {
        return Input.GetAxis("Horizontal") * _speed * Time.deltaTime;
    }

    private float GetTranslatedPosition(float _speed, float _move)
    {
        return _move * _speed * Time.deltaTime;
    }

    private void CheckPositioningConstraints()
    {
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

        if (transform.eulerAngles.y > 20f && transform.eulerAngles.y < 180f) // I was expecting the rotation to be represented in positive and negative (as in the inspector) but its not
        {
            transform.eulerAngles = new Vector3(0f, 20f, 0f);
        }
        else if (transform.eulerAngles.y < 340f && transform.eulerAngles.y > 180f)
        {
            transform.eulerAngles = new Vector3(0f, -20f, 0f);
        }
    }
}
