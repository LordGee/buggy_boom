using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsControlScript : MonoBehaviour
{

    private int gameScore, currentScore;
    private PlayerPrefsControlScript playerPrefs;
    private Text earned, total;

	// Use this for initialization
	void Start () {
        playerPrefs = FindObjectOfType<PlayerPrefsControlScript>();
        gameScore = playerPrefs.GetGameMoney();
        currentScore = playerPrefs.GetCurrentGameMoney();
	    currentScore += gameScore;
	    earned = GameObject.Find("Txt_GameScore").GetComponent<Text>();
	    total = GameObject.Find("Txt_CurrentGameScore").GetComponent<Text>();
	    earned.text = gameScore.ToString();
	    total.text = currentScore.ToString();
	    playerPrefs.SetAccumalitiveMoney(playerPrefs.GetAccumalitiveMoney() + gameScore);
        gameScore = 0;
	    playerPrefs.SetGameMoney(gameScore);
	    playerPrefs.SetCurrentGameMoney(currentScore);
    }
}
