using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShredderBack : MonoBehaviour {

    private void OnTriggerEnter(Collider col)
    {
        Destroy(col.gameObject);
        if (col.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(2);
        }
    }
}
