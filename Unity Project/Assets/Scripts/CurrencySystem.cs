using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencySystem : MonoBehaviour {

	public static int HighScore;

    public static int CoinAmount;

	public static string[] PowerUps = { "", "", "" };

    public void Awake()
    {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
       

        CoinAmount = PlayerPrefs.GetInt("Coins");
		HighScore = PlayerPrefs.GetInt ("Highscore");

		PowerUps [0] = PlayerPrefs.GetString ("Equipment1");
		PowerUps [1] = PlayerPrefs.GetString ("Equipment2");
		PowerUps [2] = PlayerPrefs.GetString ("Equipment3");

        for (int i = 0; i < StoryProgress.score.Length; i++)
        {
            StoryProgress.score[i] = PlayerPrefs.GetInt("StoryScore" + i);
        }

        for (int i = 0; i < StoryProgress.LevelStars.Length; i++)
        {
            StoryProgress.LevelStars[i] = PlayerPrefs.GetInt("StoryStar" + i);
        }

        
       
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(CoinAmount); 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("SAVES CLEARED");
            PlayerPrefs.DeleteAll();

        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Coins", CoinAmount);
		PlayerPrefs.SetInt ("Highscore", HighScore);
		PlayerPrefs.SetString ("Equipment1", PowerUps [0]);
		PlayerPrefs.SetString ("Equipment2", PowerUps [1]);
		PlayerPrefs.SetString ("Equipment3", PowerUps [2]);

        for (int i = 0; i < StoryProgress.score.Length; i++)
        {
            PlayerPrefs.SetInt("StoryScore" + i, StoryProgress.score[i]);
        }

        for (int i = 0; i < StoryProgress.LevelStars.Length; i++)
        {
            PlayerPrefs.SetInt("StoryStar" + i, StoryProgress.LevelStars[i]);
        }
    }

}
