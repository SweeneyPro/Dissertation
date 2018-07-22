using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;


public class Analytics : MonoBehaviour {

	// Use this for initialization
	void Start () {
		UnityEngine.Analytics.Analytics.Transaction ("test", 1, "mon");
		UnityEngine.Analytics.Analytics.CustomEvent ("DATA", new Vector3(Random.Range(20,30), Random.Range(50000, 100000), Random.Range(1,10)));
		UnityEngine.Analytics.Analytics.CustomEvent ("TEST");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
