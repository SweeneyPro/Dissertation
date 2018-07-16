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

	}
	
	// Update is called once per frame
	void Update () {

        Debug.Log(CoinAmount); 
        
	}

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Coins", CoinAmount);
    }

}
