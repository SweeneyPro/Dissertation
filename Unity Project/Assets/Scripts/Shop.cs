using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

    [SerializeField]
    private GameObject[] ShopSelection;

    [SerializeField]
    private GameObject PanelObject;

	[SerializeField]
	private GameObject[] Equipped;

	[SerializeField]
	private Sprite Shuffle;

	[SerializeField]
	private Sprite Time;

	[SerializeField]
	private Sprite Powerup;


	[SerializeField]
	private Text CoinAmountText;


	// Use this for initialization
	void Start () {
		UpdateCoinAmount ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void UpdateCoinAmount()
	{
		CoinAmountText.text = CurrencySystem.CoinAmount.ToString();
	}

    public void OpenShopItem(int ShopItem)
    {
        ShopSelection[ShopItem].SetActive(true);
        PanelObject.SetActive(true);
    }

    public void ShutShopItem(int ShopItem)
    {
        ShopSelection[ShopItem].SetActive(false);
        PanelObject.SetActive(false);
    }

	public void BuyShuffle(int ItemPrice)
    {
		if(CurrencySystem.CoinAmount >= ItemPrice)
        {
            

			if (CurrencySystem.PowerUps [0] == "") {

				CurrencySystem.CoinAmount -= ItemPrice;
				CurrencySystem.PowerUps [0] = "Shuffle";
				GetComponent<AudioSource> ().Play ();


			} else if (CurrencySystem.PowerUps [1] == "") {
				CurrencySystem.CoinAmount -= ItemPrice;
				CurrencySystem.PowerUps [1] = "Shuffle";
				GetComponent<AudioSource> ().Play ();

			}
			else if (CurrencySystem.PowerUps [2] == "") {
				CurrencySystem.CoinAmount -= ItemPrice;
				CurrencySystem.PowerUps [2] = "Shuffle";
				GetComponent<AudioSource> ().Play ();

			}
			EquipItem ();
        }
		UpdateCoinAmount ();
    }

	public void BuyTimeBoost(int ItemPrice)
	{
		if(CurrencySystem.CoinAmount >= ItemPrice)
		{


			if (CurrencySystem.PowerUps [0] == "") {

				CurrencySystem.CoinAmount -= ItemPrice;
				CurrencySystem.PowerUps [0] = "Time";
				GetComponent<AudioSource> ().Play ();

			} else if (CurrencySystem.PowerUps [1] == "") {
				CurrencySystem.CoinAmount -= ItemPrice;
				CurrencySystem.PowerUps [1] = "Time";
				GetComponent<AudioSource> ().Play ();
			}
			else if (CurrencySystem.PowerUps [2] == "") {
				CurrencySystem.CoinAmount -= ItemPrice;
				CurrencySystem.PowerUps [2] = "Time";
				GetComponent<AudioSource> ().Play ();
			}
			EquipItem ();
		}

		UpdateCoinAmount ();
	}

	public void BuyGemPowerUp(int ItemPrice)
	{
		if(CurrencySystem.CoinAmount >= ItemPrice)
		{


			if (CurrencySystem.PowerUps [0] == "") {

				CurrencySystem.CoinAmount -= ItemPrice;
				CurrencySystem.PowerUps [0] = "PowerUp";
				GetComponent<AudioSource> ().Play ();

			} else if (CurrencySystem.PowerUps [1] == "") {
				CurrencySystem.CoinAmount -= ItemPrice;
				CurrencySystem.PowerUps [1] = "PowerUp";
				GetComponent<AudioSource> ().Play ();
			}
			else if (CurrencySystem.PowerUps [2] == "") {
				CurrencySystem.CoinAmount -= ItemPrice;
				CurrencySystem.PowerUps [2] = "PowerUp";
				GetComponent<AudioSource> ().Play ();
			}

			EquipItem ();

		}
		UpdateCoinAmount ();
	}

	private void EquipItem()
	{

		for (int i = 0; i < CurrencySystem.PowerUps.Length; i++) {

			if (CurrencySystem.PowerUps [i] == "Shuffle") {

				Equipped [i].GetComponent<Image> ().sprite = Shuffle;

			} else if (CurrencySystem.PowerUps [i] == "Time") {

				Equipped [i].GetComponent<Image> ().sprite = Time;

			} else if (CurrencySystem.PowerUps [i] == "PowerUp") {

				Equipped [i].GetComponent<Image> ().sprite = Powerup;

			}


		}
	}
}
