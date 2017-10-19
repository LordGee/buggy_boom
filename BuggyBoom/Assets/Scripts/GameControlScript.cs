using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Array = UnityScript.Lang.Array;
using Random = UnityEngine.Random;

public class GameControlScript : MonoBehaviour
{

    public float playerHealth = 100f;
    public float playerDamage = 10f;

    public GameObject[] npcGameObjects;

    public float npcJeepHealth = 10f;
    public float npcJeepDamage = 10f;
    public float npcJeepSpeed = 10f;

    public float npcBlockHealth = 999999f;
    public float npcBlockDamage = 999999f;
    public float npcBlockSpeed = 12f;

    public float npcMonsterHealth = 1000f;
    public float npcMonsterDamage = 20f;
    public float npcMonsterSpeed = 2f;

    private float playerScore, playerMultipler, playerPoints;
    private float progressionTimer, progressionCountdown, progressionIncrementer;
    private float bossTimer, bossCountdown;
    private const float maxSpeed = 30f;
    private bool invinsible = false;

    private Text scoreDisplay, multiDisplay, healthDisplay;

    private int[] roadLaneArray = { -3, -1, 1, 3 };
    private int[] fullLaneArray = { -5, -3, -1, 1, 3, 5 };
    private int[] bossLaneArray = { 0 };

    public enum SPAWN_NPC { Jeep, Block, Boss };
    public SPAWN_NPC currentSpawn;

    public GameObject[] collectables;



	// Use this for initialization
	void Start ()
	{
	    playerScore = 0f;
	    playerMultipler = 1f;
	    playerPoints = 100f;
	    progressionTimer = Time.timeSinceLevelLoad;
	    progressionIncrementer = 0.1f;
	    progressionCountdown = 8f;
	    bossTimer = Time.timeSinceLevelLoad;
	    bossCountdown = 10f;
	    currentSpawn = SPAWN_NPC.Jeep;
        scoreDisplay = GameObject.Find("Text_ScoreDisplay").GetComponent<Text>();
	    multiDisplay = GameObject.Find("Text_MultiplerDisplay").GetComponent<Text>();
	    healthDisplay = GameObject.Find("Text_HealthDisplay").GetComponent<Text>();
        UpdateHUD();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (currentSpawn != SPAWN_NPC.Boss)
	    {
	        if (Time.timeSinceLevelLoad - progressionTimer > progressionCountdown)
	        {
	            ProgressionCalc(ref npcJeepHealth);
	            ProgressionCalc(ref playerMultipler);
	            ProgressionCalc(ref progressionCountdown);
	            if (npcJeepSpeed <= maxSpeed)
	            {
	                ProgressionCalc(ref npcJeepSpeed);
	            }
	            if (currentSpawn == SPAWN_NPC.Jeep)
	            {
	                currentSpawn = SPAWN_NPC.Block;
	            }
	            else
	            {
	                currentSpawn = SPAWN_NPC.Jeep;
	            }
	            UpdateHUD();
	            progressionTimer = Time.timeSinceLevelLoad;
	        }
	        if (Time.timeSinceLevelLoad - bossTimer > bossCountdown)
	        {
	            currentSpawn = SPAWN_NPC.Boss;
	        }

        }
	    else
	    {
	        
	    }
	}

    public void DamagePlayer(float _dmg)
    {
        if (!invinsible)
        {
            playerHealth -= _dmg;
            playerMultipler = 1;
            invinsible = !invinsible;
        }
        StartCoroutine(PlayerHit());
        if (playerHealth <= 0)
        {
            GameOver();
        }
        UpdateHUD();
    }

    private IEnumerator PlayerHit()
    {
        yield return new WaitForSeconds(1.0f);
        invinsible = false;
    }

    public void DamageNPC(GameObject _obj, float _pts, float _dmg, ref float _hea)
    {
        _hea -= _dmg;
        if (_hea <= 0)
        {
            AddPoints(playerPoints);
            if (currentSpawn == SPAWN_NPC.Boss)
            {
                for (int i = 0; i < Mathf.Floor(playerMultipler * 5); i++)
                {
                    SpawnCollectable(new Vector3(_obj.transform.position.x + Random.Range(-1f, 1f), _obj.transform.position.y, _obj.transform.position.z + Random.Range(-1f, 1f)));
                }
                currentSpawn = SPAWN_NPC.Jeep;
                bossTimer = Time.timeSinceLevelLoad;
                LevelUp();
            }
            else
            {
                SpawnCollectable(_obj.transform.position);
            }
            Destroy(_obj);
        }
    }

    private void LevelUp()
    {
        npcJeepDamage += 5;
        npcMonsterDamage += 5;
        playerDamage += 5;
        npcMonsterHealth += 500;
        playerMultipler *= 2;
    }

    public void AddPoints(float _pts)
    {
        playerScore += Mathf.Floor(_pts * playerMultipler);
        UpdateHUD();
    }

    public void RepairBuggy(float _value)
    {
        playerHealth += _value;
        if (playerHealth > 100)
        {
            AddPoints(playerHealth - 100);
            playerHealth = 100;
        }
        UpdateHUD();
    }

    private void SpawnCollectable(Vector3 pos)
    {
        int spawnValue = -1;
        bool next = true;
        for (int i = 0; i < collectables.Length; i++)
        {
            if (Random.Range(0, 2) == 1 && next)
            {
                spawnValue = i;
            }
            else
            {
                next = false;
            }
        }
        if (spawnValue != -1)
        {
            Instantiate(collectables[spawnValue], pos, Quaternion.identity);
        }
    }

    private void UpdateHUD()
    {
        scoreDisplay.text = playerScore.ToString();
        multiDisplay.text = playerMultipler.ToString("F1") + "x";
        SetHealthColour();
        healthDisplay.text = playerHealth.ToString("F0") + "%";
    }

    private void ProgressionCalc(ref float _value)
    {
        _value += progressionIncrementer;
    }

    public float GetPlayerHealth() { return playerHealth; }
    public float GetPlayerDamage() { return playerDamage; }
    public float GetPlayerPoints() { return playerPoints; }

    public float GetNpcHealth()
    {
        return (currentSpawn == SPAWN_NPC.Jeep) ? npcJeepHealth : (currentSpawn == SPAWN_NPC.Block) ? npcBlockHealth : (currentSpawn == SPAWN_NPC.Boss) ? npcMonsterHealth : 0; 
    }

    public float GetNpcDamage()
    {
        return (currentSpawn == SPAWN_NPC.Jeep) ? npcJeepDamage : (currentSpawn == SPAWN_NPC.Block) ? npcBlockHealth : (currentSpawn == SPAWN_NPC.Boss) ? npcMonsterDamage : 0;
    }

    public float GetNpcSpeed()
    {
        return (currentSpawn == SPAWN_NPC.Jeep) ? npcJeepSpeed : (currentSpawn == SPAWN_NPC.Block) ? npcBlockSpeed : (currentSpawn == SPAWN_NPC.Boss) ? npcMonsterSpeed : 0;
    }

    public GameObject GetNpcGameObjectToSpawn()
    {
        return (currentSpawn == SPAWN_NPC.Jeep) ? npcGameObjects[0] : (currentSpawn == SPAWN_NPC.Block) ? npcGameObjects[1] : (currentSpawn == SPAWN_NPC.Boss) ? npcGameObjects[2] : npcGameObjects[0];
    }

    public int[] GetLaneArray()
    {
        return (currentSpawn == SPAWN_NPC.Jeep) ? roadLaneArray : (currentSpawn == SPAWN_NPC.Block) ? fullLaneArray : (currentSpawn == SPAWN_NPC.Boss) ? bossLaneArray : roadLaneArray;
    }

    private void SetHealthColour()
    {
        if (playerHealth > 50)
        {
            healthDisplay.color = Color.green;
        }
        else if (playerHealth <= 50 && playerHealth > 25)
        {
            healthDisplay.color = Color.yellow;
        }
        else if (playerHealth <= 25)
        {
            healthDisplay.color = Color.red;
        }
        else
        {
            healthDisplay.color = Color.black;
        }
    }

    void GameOver()
    {
        Debug.LogError("You Broke the Game by DYING!");
    }
}
