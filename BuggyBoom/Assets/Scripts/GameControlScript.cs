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

    private float playerScore, playerMultipler, playerPoints;
    private float progressionTimer, progressionCountdown, progressionIncrementer;
    private const float maxSpeed = 30f;
    private bool invinsible = false;

    private Text scoreDisplay, multiDisplay, healthDisplay;

    private int[] roadLaneArray = { -3, -1, 1, 3 };
    private int[] fullLaneArray = { -5, -3, -1, 1, 3, 5 };

    private enum SPAWN_NPC { Jeep, Block };
    private SPAWN_NPC currentSpawn;

    public float npcSpawnDuration = 15f;

	// Use this for initialization
	void Start ()
	{
	    playerScore = 0f;
	    playerMultipler = 1f;
	    playerPoints = 100f;
	    progressionTimer = Time.timeSinceLevelLoad;
	    progressionIncrementer = 0.1f;
	    progressionCountdown = 8f;
	    currentSpawn = SPAWN_NPC.Jeep;
        scoreDisplay = GameObject.Find("Text_ScoreDisplay").GetComponent<Text>();
	    multiDisplay = GameObject.Find("Text_MultiplerDisplay").GetComponent<Text>();
	    healthDisplay = GameObject.Find("Text_HealthDisplay").GetComponent<Text>();
        UpdateHUD();
    }
	
	// Update is called once per frame
	void Update ()
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
            Destroy(_obj);
        }
    }

    public void AddPoints(float _pts)
    {
        playerScore += Mathf.Floor(_pts * playerMultipler);
        UpdateHUD();
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
        return (currentSpawn == SPAWN_NPC.Jeep) ? npcJeepHealth : (currentSpawn == SPAWN_NPC.Block) ? npcBlockHealth : 0; 
    }

    public float GetNpcDamage()
    {
        return (currentSpawn == SPAWN_NPC.Jeep) ? npcJeepDamage : (currentSpawn == SPAWN_NPC.Block) ? npcBlockHealth : 0;
    }

    public float GetNpcSpeed()
    {
        return (currentSpawn == SPAWN_NPC.Jeep) ? npcJeepSpeed : (currentSpawn == SPAWN_NPC.Block) ? npcBlockSpeed : 0;
    }

    public GameObject GetNpcGameObjectToSpawn()
    {
        return (currentSpawn == SPAWN_NPC.Jeep) ? npcGameObjects[0] : (currentSpawn == SPAWN_NPC.Block) ? npcGameObjects[1] : npcGameObjects[0];
    }

    public int[] GetLaneArray()
    {
        return (currentSpawn == SPAWN_NPC.Jeep) ? roadLaneArray : (currentSpawn == SPAWN_NPC.Block) ? fullLaneArray : roadLaneArray;
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
