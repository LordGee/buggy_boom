using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawn : MonoBehaviour
{

    public float projectileSpeed = 20f;

    private GameObject buggyRotation;
    private Vector3 originalForward;

    // Use this for initialization
    void Start()
    {
        buggyRotation = GameObject.FindGameObjectWithTag("Player");
        originalForward = buggyRotation.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(originalForward * projectileSpeed * Time.deltaTime);
        // transform.Translate(0f,0f, projectileSpeed * Time.deltaTime);
        // transform.Rotate(0f, buggyRotation.transform.rotation.y * Time.deltaTime, 0f);
        // print(buggyRotation.transform.eulerAngles.y);
    }
}
