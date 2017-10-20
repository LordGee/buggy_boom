using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarsCollect : MonoBehaviour {

    private float speed = 8f;
    private int pointValue = 1000;
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
        gameControl.AddPoints(pointValue);
        Destroy(gameObject);
    }
}
