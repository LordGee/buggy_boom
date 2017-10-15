using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeepNPC : MonoBehaviour {

    public Material[] materials;

    private int[] roadLaneArray = { -3, -1, 1, 3 };

    // Use this for initialization
    void Start()
    {
        GameObject obj = transform.Find("JEEP_BODY").gameObject;
        obj.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
    }

    public int[] GetLaneArray()
    {
        return roadLaneArray;
    }
}
