﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorPiece : MonoBehaviour {

	public enum ColorType
	{
		DEATH,
		EARTH,
		FIRE,
		LIFE,
		WATER,
		WIND,
		ANY,
		COUNT
	};

	[System.Serializable]
	public struct ColorSprite
	{
		public ColorType color;
		public Sprite sprite;
	};

	public ColorSprite[] colorSprites;

	private ColorType color;

	public ColorType Color
	{
		get { return color; }
		set { SetColor (value); }
	}

	public int NumColors
	{
		get { return colorSprites.Length; }
	}

	private SpriteRenderer sprite;
	private Dictionary<ColorType, Sprite> colorSpriteDict;

	void Awake()
	{
		sprite = transform.Find ("piece").GetComponent<SpriteRenderer> ();

		colorSpriteDict = new Dictionary<ColorType, Sprite> ();

		for (int i = 0; i < colorSprites.Length; i++) {
			if (!colorSpriteDict.ContainsKey (colorSprites [i].color)) {
				colorSpriteDict.Add (colorSprites [i].color, colorSprites [i].sprite);
			}
		}
	}

	// Use this for initialization
	void Start () {
	
		//Debug.Log (Color.ToString ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetColor(ColorType newColor)
	{
		color = newColor;

		if (colorSpriteDict.ContainsKey (newColor)) {
			sprite.sprite = colorSpriteDict [newColor];
		}
	}

	public ColorType RandomType()
	{
		int random = Random.Range (0, 6);

		switch (random) 
		{
		case 0:
			return ColorType.DEATH;
			break;
		case 1:
			return ColorType.EARTH;
			break;
		case 2:
			return ColorType.FIRE;
			break;
		case 3:
			return ColorType.LIFE;
			break;
		case 4:
			return ColorType.WATER;
			break;
		case 5:
			return ColorType.WIND;
			break;

		}


		return ColorType.DEATH;
	}




}
