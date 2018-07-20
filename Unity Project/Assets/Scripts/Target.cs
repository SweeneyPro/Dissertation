using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Target : MonoBehaviour {

	public Level levelobjects;

	public Text targetone;
	public Text targettwo;
	public Text targetthree;

	// Use this for initialization
	void Start () {

		targetone.text = levelobjects.score1Star.ToString ();
		targettwo.text = levelobjects.score2Star.ToString ();
		targetthree.text = levelobjects.score3Star.ToString ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
