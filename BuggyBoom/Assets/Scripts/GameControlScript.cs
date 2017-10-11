using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControlScript : MonoBehaviour
{

    public float playerHealth = 100f;
    public float playerDamage = 10f;

    public float npcJeepHealth = 10f;
    public float npcJeepDamage = 10f;
    public float npcJeepSpeed = 10f;

    private float playerScore, playerMultipler, playerPoints;
    private float progressionTimer, progressionCountdown, progressionIncrementer;
    private const float maxSpeed = 30f;

	// Use this for initialization
	void Start ()
	{
	    playerScore = 0f;
	    playerMultipler = 1f;
	    playerPoints = 100f;
	    progressionTimer = Time.timeSinceLevelLoad;
	    progressionIncrementer = 0.1f;
	    progressionCountdown = 12f;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Time.timeSinceLevelLoad - progressionTimer > progressionCountdown)
	    {
            ProgressionCalc(ref npcJeepHealth);
	        ProgressionCalc(ref playerMultipler);
	        ProgressionCalc(ref progressionCountdown);
	        if (npcJeepSpeed <= maxSpeed)
	        {
	            ProgressionCalc(ref npcJeepSpeed);
	        }
	        progressionTimer = Time.timeSinceLevelLoad;
        }
	}

     public void DamagePlayer(float _dmg)
    {
        playerHealth -= _dmg;
        if (playerHealth <= 0)
        {
            GameOver();
        }
    }

    public void DamageNPC(GameObject _obj, float _dmg, ref float _hea)
    {
        _hea -= _dmg;
        if (_hea <= 0)
        {
            AddPoints(playerScore);
            Destroy(_obj);
        }
    }

    public void AddPoints(float _pts)
    {
        playerScore += _pts * playerMultipler;
    }

    private void ProgressionCalc(ref float _value)
    {
        _value += _value * progressionIncrementer;
    }

    public float GetPlayerHealth() { return playerHealth; }
    public float GetPlayerDamage() { return playerDamage; }

    public float GetNpcJeepHealth() { return npcJeepHealth; }
    public float GetNpcJeepDamage() { return npcJeepDamage; }
    public float GetNpcJeepSpeed() { return npcJeepSpeed; }

    void GameOver()
    {
        Debug.LogError("You Broke the Game by DYING!");
    }
}
