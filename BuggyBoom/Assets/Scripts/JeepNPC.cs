using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeepNPC : MonoBehaviour {

    public Material[] materials;
    public GameObject projectile;
    private int[] roadLaneArray = { -3, -1, 1, 3 };
    private bool shooter = false;
    private float shootTimer;
    private float shootFreq = 3f;
    public float projectileLife = 1.75f;


    // Use this for initialization
    void Start()
    {
        GameObject obj = transform.Find("JEEP_BODY").gameObject;
        obj.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        if (Random.Range(0,3) == 1)
        {
            shooter = true;
            shootTimer = Time.timeSinceLevelLoad;
        }
    }

    void Update()
    {
        if (shooter && Time.timeSinceLevelLoad - shootTimer > shootFreq)
        {
            GameObject proj = Instantiate(projectile, 
                new Vector3(transform.position.x, transform.position.y, transform.position.z - 1),
                Quaternion.identity);
            Destroy(proj, 3f);
            shootTimer = Time.timeSinceLevelLoad;
        }
    }

    public int[] GetLaneArray()
    {
        return roadLaneArray;
    }
}
