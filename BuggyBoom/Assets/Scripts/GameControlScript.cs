using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameControlScript : MonoBehaviour
{
    // Player Specific Public Variables
    public float playerHealth = 100f;
    public float playerDamage = 10f;

    // NPC Specific Public Variables
    public GameObject[] npcGameObjects;
    public float npcJeepHealth = 10f;
    public float npcJeepDamage = 2f;
    public float npcJeepSpeed = 10f;
    public float npcJeepPoints = 100f;
    public float npcBlockHealth = 999999f;
    public float npcBlockDamage = 999999f;
    public float npcBlockSpeed = 12f;
    public float npcBlockPoints = 0f;
    public float npcMonsterHealth = 1000f;
    public float npcMonsterDamage = 5f;
    public float npcMonsterSpeed = 5f;
    public float npcMonsterPoints = 1000f;
    public enum SPAWN_NPC { Jeep, Block, Boss };
    public SPAWN_NPC currentSpawn;

    // Game Specific Public Variables
    public enum GAME_STATE { Playing, Paused, GameOver };
    public GAME_STATE currentyGameState;
    public GameObject[] collectables;
    public GameObject explodeEffect;

    // Player Specific Private Variables
    private float playerScore, playerMultipler, playerPoints;
    private bool invinsible = false;  

    // NPC Specific Private Variables
    private float bossTimer, bossCountdown;
    private int[] roadLaneArray = { -3, -1, 1, 3 };
    private int[] fullLaneArray = { -5, -3, -1, 1, 3, 5 };
    private int[] bossLaneArray = { 0 };

    // Game Specific Private Variables
    private float progressionTimer, progressionCountdown, progressionIncrementer;
    private const float maxSpeed = 30f;
    private Text scoreDisplay, multiDisplay, healthDisplay;

	// Use this for initialization
	void Start ()
	{
	    currentyGameState = GAME_STATE.Playing;
	    playerScore = 0f;
	    playerMultipler = 1f;
	    playerPoints = 100f; // Default Amount
	    progressionTimer = Time.timeSinceLevelLoad;
	    progressionIncrementer = 0.1f;
	    progressionCountdown = 10f;
	    bossTimer = Time.timeSinceLevelLoad;
	    bossCountdown = 20f;
	    currentSpawn = SPAWN_NPC.Jeep;
        scoreDisplay = GameObject.Find("Text_ScoreDisplay").GetComponent<Text>();
	    multiDisplay = GameObject.Find("Text_MultiplerDisplay").GetComponent<Text>();
	    healthDisplay = GameObject.Find("Text_HealthDisplay").GetComponent<Text>();
        UpdateHUD();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    switch (currentyGameState)
	    {
            case GAME_STATE.Playing :
                StartGameChecks();
                break;
            case GAME_STATE.Paused:
                PauseGame();
                break;
            case GAME_STATE.GameOver:
                GameOver();
                break;
	    }
	}

    void StartGameChecks()
    {
        if (currentSpawn != SPAWN_NPC.Boss && !(GameObject.FindWithTag("NPCBoss")))
        {
            if (Time.timeSinceLevelLoad - progressionTimer > progressionCountdown)
            {
                NextWaveUpdate();
            }
        }
    }

    /* Updates the next wave variables, to make the experience progressively harder, 
     * more rewarding and changes the current spawn objecct. */
    private void NextWaveUpdate()
    {
        ProgressionCalc(ref npcJeepHealth);
        ProgressionCalc(ref playerMultipler);
        ProgressionCalc(ref progressionCountdown);
        if (npcJeepSpeed <= maxSpeed)
        {
            ProgressionCalc(ref npcJeepSpeed);
        }
        if (Time.timeSinceLevelLoad - bossTimer > bossCountdown)
        {
            currentSpawn = SPAWN_NPC.Boss;
        }
        else
        {
            if (currentSpawn == SPAWN_NPC.Jeep)
            {
                currentSpawn = SPAWN_NPC.Block;
            }
            else
            {
                currentSpawn = SPAWN_NPC.Jeep;
            }
        }
        UpdateHUD();
        progressionTimer = Time.timeSinceLevelLoad;
    }

    /* Updates the values of the players health when taking damage, also resets the 
     * multipler and sets a short time where the player is invinsible via a coroutine. 
     * If the players gets to zero the game state is changed to the Game Over state. */
    public void DamagePlayer(float _dmg)
    {
        if (!invinsible)
        {
            playerHealth -= _dmg;
            playerMultipler = 1;
            invinsible = !invinsible;
            StartCoroutine(PlayerHit());
        }
        if (playerHealth <= 0)
        {
            currentyGameState = GAME_STATE.GameOver;
        }
        UpdateHUD();
    }

    /* Provides a short delay be take additional damage */
    private IEnumerator PlayerHit()
    {
        yield return new WaitForSeconds(0.2f);
        invinsible = false;
    }

    /* Manages the damage to an NPC object, reducing its current health. If health is below 
     * zero will increment the players by the passed in amount of points if the object was a 
     * boss then additional changes are set and bigger rewards are provided, level up takes 
     * place to make the game incrementally harder, plus bigger rewards. The current spawn 
     * will return to the jeep. Else if not a boss then one reward is spawned. Also now managed 
     * the explosion effect, which reduces the amount of reat code in other scripts. */
    public void DamageNPC(GameObject _obj, float _pts, float _dmg, ref float _hea)
    {
        _hea -= _dmg;
        if (_hea <= 0)
        {
            AddPoints(_pts);
            if (currentSpawn == SPAWN_NPC.Boss && GameObject.FindWithTag("NPCBoss"))
            {
                for (int i = 0; i < Mathf.Floor(playerMultipler * 5); i++)
                {
                    SpawnCollectable(new Vector3(_obj.transform.position.x + Random.Range(-2.0f, 2.0f), _obj.transform.position.y, _obj.transform.position.z + Random.Range(-2.0f, 2.0f)));
                }
                bossTimer = Time.timeSinceLevelLoad;
                LevelUp();
                progressionTimer = Time.timeSinceLevelLoad;
                currentSpawn = SPAWN_NPC.Jeep;
            }
            else
            {
                SpawnCollectable(_obj.transform.position);
            }
            NpcDeathEffect(_obj.transform.position);
            Destroy(_obj);
            UpdateHUD();
        }
    }

    /* Spawns an explosion effect when NPC is destroyed */
    void NpcDeathEffect(Vector3 pos)
    {
        GameObject explode = Instantiate(explodeEffect, pos, Quaternion.identity);
        Destroy(explode, 3f);
    }

    /* After defeating a boss the following variables are increased */
    private void LevelUp()
    {
        npcJeepDamage += 2;
        npcJeepHealth += 10;
        npcMonsterDamage += 5;
        npcMonsterHealth += 500;
        playerDamage += 5;
        playerMultipler *= 2;
        UpdateHUD();
    }

    /* Addded set points multipled by the game multipler to the players score. */
    public void AddPoints(float _pts)
    {
        playerScore += Mathf.Floor(_pts * playerMultipler);
        UpdateHUD();
    }

    /* Repairs the buggy by a set value and enforces a maximum player health. */
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

    /* Spawns a pick up item as a reward ffor the player, the type of pick up 
     * is determined using a bingo style system, ensuring the more desirable 
     * pick ups are less likely, but not impossible. */
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

    /* Updates the text displayed within the HUD canvas */
    private void UpdateHUD()
    {
        scoreDisplay.text = playerScore.ToString();
        multiDisplay.text = playerMultipler.ToString("F1") + "x";
        SetHealthColour();
        healthDisplay.text = playerHealth.ToString("F0") + "%";
    }

    /* Sets the text color for the health value depending on the value. */
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
    }

    /* Increments variables and certain times within the game. */
    private void ProgressionCalc(ref float _value)
    {
        _value += progressionIncrementer;
    }

    /* Getters */
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

    public float GetNpcPoints()
    {
        return (currentSpawn == SPAWN_NPC.Jeep) ? npcJeepPoints : (currentSpawn == SPAWN_NPC.Block) ? npcBlockPoints : (currentSpawn == SPAWN_NPC.Boss) ? npcMonsterPoints : 0;
    }

    public GameObject GetNpcGameObjectToSpawn()
    {
        return (currentSpawn == SPAWN_NPC.Jeep) ? npcGameObjects[0] : (currentSpawn == SPAWN_NPC.Block) ? npcGameObjects[1] : (currentSpawn == SPAWN_NPC.Boss) ? npcGameObjects[2] : npcGameObjects[0];
    }

    public int[] GetLaneArray()
    {
        return (currentSpawn == SPAWN_NPC.Jeep) ? roadLaneArray : (currentSpawn == SPAWN_NPC.Block) ? fullLaneArray : (currentSpawn == SPAWN_NPC.Boss) ? bossLaneArray : roadLaneArray;
    }

    /* Called when game state changes to paused */
    void PauseGame()
    {
        // Add a pause loop here
    }

    /* End of the game, when the player is destroyed */
    void GameOver()
    {
        Debug.LogError("You Broke the Game by DYING!");
    }
}
