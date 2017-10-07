using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShredderBack : MonoBehaviour {

    private void OnTriggerEnter(Collider col)
    {
        Destroy(col.gameObject);
    }
}
