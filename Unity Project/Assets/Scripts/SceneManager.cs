using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MainMenuScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
    }

    public void LevelTypeScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameTypeScene");
    }

    public void BlitzScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Blitz");
    }

    public void PuzzleScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Puzzle");
    }

    public void SettingsScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SettingsScene");
    }

    public void StatsScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StatsScene");
    }

    public void Exit()
    {
        Application.Quit();
        
    }
}
