using UnityEngine;
using System.Collections;

public class ClearLinePiece : ClearablePiece {

	public bool isRow;

    [SerializeField]
    private GameObject[] ParticlesObjects;
    

	// Use this for initialization
	void Start () {

        ColorPiece ColourPieceComponent = GetComponent<ColorPiece>();

		switch (ColourPieceComponent.Color) 
		{

		case ColorPiece.ColorType.DEATH:
			Instantiate (ParticlesObjects [0], transform);
			//Debug.Log ("HEY");
			break;

		case ColorPiece.ColorType.EARTH:
			Instantiate (ParticlesObjects [1], transform);
			//Debug.Log ("HEY");
			break;

		case ColorPiece.ColorType.FIRE:
			Instantiate (ParticlesObjects [2], transform);
			//Debug.Log ("HEY");
			break;

		case ColorPiece.ColorType.LIFE:
			Instantiate (ParticlesObjects [3], transform);
			//Debug.Log ("HEY");
			break;

		case ColorPiece.ColorType.WATER:
			Instantiate (ParticlesObjects [4], transform);
			//Debug.Log ("HEY");
			break;

		case ColorPiece.ColorType.WIND:
			Instantiate (ParticlesObjects [5], transform);
			//Debug.Log ("HEY");
			break;
		}



	

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Clear()
	{
		base.Clear ();

		if (isRow)
		{
			piece.GridRef.ClearRow (piece.Y);
		}
		else
		{
			piece.GridRef.ClearColumn (piece.X);
		}
	}
}
