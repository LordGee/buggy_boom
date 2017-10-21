using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairCollect : MonoBehaviour {
    
    // Private Variables
    private GameControlScript gameControl;
    private float speed = 8f;
    private int repairValue = 20;

    // Use this for initialization
    void Start()
    {
        gameControl = FindObjectOfType<GameControlScript>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0f, 0f, -(speed * Time.deltaTime));
    }

    void OnTriggerEnter(Collider col)
    {
        gameControl.RepairBuggy(repairValue);
        Destroy(gameObject);
    }

}
