using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsControlScript : MonoBehaviour {

    private int gameScore;
    private int currentScore;
    private PlayerPrefsControlScript playerPrefs;

	// Use this for initialization
	void Start () {
        playerPrefs = FindObjectOfType<PlayerPrefsControlScript>();
        gameScore = playerPrefs.GetGameMoney();
        currentScore = playerPrefs.GetCurrentGameMoney();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
