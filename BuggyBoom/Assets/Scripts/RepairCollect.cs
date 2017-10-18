using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairCollect : MonoBehaviour {

    public float speed = 8f;
    public int repairValue = 20;
    private GameControlScript gameControl;

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
