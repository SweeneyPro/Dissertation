using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

    [SerializeField]
    private GameObject[] ShopSelection;

    [SerializeField]
    private GameObject PanelObject;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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

    public void BuyItem(int ItemPrice)
    {
        if(CurrencySystem.CoinAmount >= ItemPrice)
        {
            CurrencySystem.CoinAmount -= ItemPrice;
        }
    }
}
