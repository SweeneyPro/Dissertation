using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {

	public Text HighScoreSlot;

	// Use this for initialization
	void Start () {

		if (int.Parse (GetComponent<Text> ().text) > CurrencySystem.HighScore) {

			CurrencySystem.HighScore = int.Parse (GetComponent<Text> ().text);

		}

		GetComponent<Text> ().text = CurrencySystem.HighScore.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
