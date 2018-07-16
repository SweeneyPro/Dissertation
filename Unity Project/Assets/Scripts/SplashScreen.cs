using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {

        StartCoroutine(SwitchScene());

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator SwitchScene ()
    {
        yield return new WaitForSeconds(3);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene-Jamie");
    }
}
