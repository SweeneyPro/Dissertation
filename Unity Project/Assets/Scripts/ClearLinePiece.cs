using UnityEngine;
using System.Collections;

public class ClearLinePiece : ClearablePiece {

	public bool isRow;

    [SerializeField]
    private GameObject[] Particles;
    

	// Use this for initialization
	void Start () {

        ColorPiece ColourPieceComponent = GetComponent<ColorPiece>();

        //switch(ColourPieceComponent.GetComponent<sprite>)
	
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
