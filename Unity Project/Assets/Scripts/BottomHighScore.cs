﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomHighScore : MonoBehaviour {

	// Use this for initialization
	void Start () {

        GetComponent<Text>().text = "Highscore: " + CurrencySystem.HighScore;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
