using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeepNPC : MonoBehaviour {

    // Public Variables
    [Tooltip("Two Materials to define type of jeep")]
    public Material[] materials;
    [Tooltip("Projectile that the jeep shoots")]
    public GameObject projectile;

    // Private Variables
    private GameControlScript gameControl;
    private PlayerPrefsControlScript playerPrefs;
    private new AudioSource audio;
    private int[] roadLaneArray = { -3, -1, 1, 3 };
    private bool shooter, changer;
    private float actionTimer = 0f, actionFreq = 1.5f, minAction = 0.5f, maxAction = 2.5f;
    private float projectileLife = 4f;
    private float npcDamage, npcHealth, currentX, playerPosition;
    private int currentIndex, newIndex, playerIndex;
    private float minMaxPos = 0.99f;
    private NPCObstacle moverScript;
    GameObject jeepBody, player;


    // Use this for initialization
    void Start()
    {
        gameControl = FindObjectOfType<GameControlScript>();
        playerPrefs = FindObjectOfType<PlayerPrefsControlScript>();
        moverScript = FindObjectOfType<NPCObstacle>();
        audio = GetComponent<AudioSource>();
        audio.volume = playerPrefs.GetSfXVolume();
        player = GameObject.FindGameObjectWithTag("Player");
        npcDamage = gameControl.GetNpcDamage();
        npcHealth = gameControl.GetNpcHealth();
        IsShooterAndMaterial();
        DestroyGlitchedObject();
    }

    /* Randomly chooses if next spawn Jeep is a shooter this then 
     * determines which material to provide. */
    void IsShooterAndMaterial()
    {
        jeepBody = transform.Find("JEEP_BODY").gameObject;
        int whichJeep = Random.Range(0, 4);
        if (whichJeep == 1)
        {
            //shooter = true;
            changer = true;
            SetUpJeep(1);
        }
        else if (whichJeep == 2)
        {
            changer = true;
            npcDamage += npcDamage;
            currentX = gameObject.transform.position.x;
            SetUpJeep(2);
        }
        else
        {
            changer = false;
            shooter = false;
            jeepBody.GetComponent<Renderer>().material = materials[0];
        }
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad - actionTimer > actionFreq)
        {
            if (shooter)
            {
                ShootProjectile();   
                audio.Play();
            }
            else if (changer)
            {
                currentX = transform.position.x;
                if (roadLaneArray[FindCurrentIndex(currentX, 0.25f)] != roadLaneArray[newIndex])
                {
                    playerPosition = player.gameObject.transform.position.x;
                    currentIndex = FindCurrentIndex(currentX, minMaxPos);
                    playerIndex = FindCurrentIndex(playerPosition, minMaxPos);
                    if (currentIndex < playerIndex || currentIndex > playerIndex)
                    {
                        if (currentIndex > 0 && currentIndex < roadLaneArray.Length)
                        {
                            moverScript.ChangeLane(roadLaneArray[playerIndex] - roadLaneArray[currentIndex]);
                        }
                    }
                    else
                    {
                        moverScript.ChangeLane(0f);
                    }
                    newIndex = currentIndex;
                }
                else
                {
                    moverScript.ChangeLane(0f);
                }
            }
            actionTimer = Time.timeSinceLevelLoad;
            actionFreq = Random.Range(minAction, maxAction);
        }
    }

    void SetUpJeep(int _value)
    {
        jeepBody.GetComponent<Renderer>().material = materials[_value];
        actionTimer = Time.timeSinceLevelLoad;
    }

    private int FindCurrentIndex(float _posX, float _minMaxX)
    {
        int returnIndex = currentIndex;
        for (int i = 0; i < roadLaneArray.Length; i++)
        {
            if (_posX == roadLaneArray[i] 
                || _posX < roadLaneArray[i] + minMaxPos
                && _posX > roadLaneArray[i] - minMaxPos)
            {
                returnIndex = i;
            }
        }
        return returnIndex;
    }

    /* Projectile is instantiated and the unique damage for the object
     * is passed to the projectile script */
    void ShootProjectile()
    {
        GameObject proj = Instantiate(projectile,
                new Vector3(transform.position.x, transform.position.y, transform.position.z - 1),
                Quaternion.identity);
        proj.GetComponent<ProjectileSpawnEnemy>().projectileDamage = npcDamage;
        Destroy(proj, projectileLife);
    }

    public int[] GetLaneArray()
    {
        return roadLaneArray;
    }

    /* After a boss and before road blocks a glitched Jeep is spawned with is 
     * super doper powerful, it has the same attributes as a road block so if 
     * crashed into or hits with projectile will instantly kill the player. 
     * This is a tempory fix. */
    private void DestroyGlitchedObject()
    {
        if (npcHealth > 10000)
        {
            Destroy(gameObject);
        }
    }
}
