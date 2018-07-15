using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManager : MonoBehaviour {

	AudioClip ButtonNoise;

	// Use this for initialization
	void Start () {


		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator ChangeScene(string scene){
		yield return new WaitForSeconds (0.2f);
		UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
	}

    public void MainMenuScene()
    {
		GetComponent<AudioSource> ().Play ();

		StartCoroutine(ChangeScene("MainMenuScene-Jamie"));


    }

    public void LevelTypeScene()
    {
		GetComponent<AudioSource> ().Play ();
		StartCoroutine(ChangeScene("GameTypeScene"));

    }

    public void BlitzScene()
    {
		GetComponent<AudioSource> ().Play ();
		StartCoroutine(ChangeScene("Blitz"));
    }

    public void PuzzleScene()
    {
		GetComponent<AudioSource> ().Play ();
		StartCoroutine(ChangeScene("Puzzle"));
    }

    public void SettingsScene()
    {
		GetComponent<AudioSource> ().Play ();
		StartCoroutine(ChangeScene("SettingsScene"));
    }

    public void StatsScene()
    {
		GetComponent<AudioSource> ().Play ();
		StartCoroutine(ChangeScene("StatsScene"));
    }

    public void ShopScene()
    {
		GetComponent<AudioSource> ().Play ();
		StartCoroutine(ChangeScene("Shop"));
    }

	public void LevelSelectScene()
	{
		GetComponent<AudioSource> ().Play ();
		StartCoroutine(ChangeScene("levelSelect"));
	}

    public void Exit()
    {
        Application.Quit();
        
    }
}
