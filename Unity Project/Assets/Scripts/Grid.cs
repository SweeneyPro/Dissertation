using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Grid : MonoBehaviour {

	public List<GameObject> AdjacentObjects = new List<GameObject>();

	public GameObject LightningObject;

	[SerializeField]
	private AudioClip WrongClearSound;

	public GameObject[] PowerUpIcons;

	public Sprite[] PowerUpAssets;

	public Sprite NoGem;

	private struct PiecePair
	{
		public GamePiece A;
		public GamePiece B;
	}

	public enum PieceType
	{
		EMPTY,
		NORMAL,
		BUBBLE,
		ROW_CLEAR,
		COLUMN_CLEAR,
		RAINBOW,
		COUNT,
	};

	[System.Serializable]
	public struct PiecePrefab
	{
		public PieceType type;
		public GameObject prefab;
	};

	[System.Serializable]
	public struct PiecePosition
	{
		public PieceType type;
		public int x;
		public int y;
	};

	public int xDim;
	public int yDim;
	public float fillTime;

	public Level level;

	public PiecePrefab[] piecePrefabs;
	public GameObject backgroundPrefab;

	public PiecePosition[] initialPieces;

	private Dictionary<PieceType, GameObject> piecePrefabDict;

	[SerializeField]
	private GamePiece[,] pieces;

	private bool inverse = false;

	private GamePiece pressedPiece;
	private GamePiece enteredPiece;

	private bool gameOver = false;

	private bool isFilling = false;

	public bool IsFilling
	{
		get { return isFilling; }
	}

	void Awake () {
		piecePrefabDict = new Dictionary<PieceType, GameObject> ();

		for (int i = 0; i < piecePrefabs.Length; i++) {
			if (!piecePrefabDict.ContainsKey (piecePrefabs [i].type)) {
				piecePrefabDict.Add (piecePrefabs [i].type, piecePrefabs [i].prefab);
			}
		}
		/*
		for (int x = 0; x < xDim; x++) {
			for (int y = 0; y < yDim; y++) {
				GameObject background = (GameObject)Instantiate (backgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);
				background.transform.parent = transform;
			}
		}
*/
		pieces = new GamePiece[xDim, yDim];

		for (int i = 0; i < initialPieces.Length; i++) {
			if (initialPieces [i].x >= 0 && initialPieces [i].x < xDim
			    && initialPieces [i].y >= 0 && initialPieces [i].y < yDim) {
				SpawnNewPiece (initialPieces [i].x, initialPieces [i].y, initialPieces [i].type);
			}
		}

		for (int x = 0; x < xDim; x++) {
			for (int y = 0; y < yDim; y++) {
				if (pieces [x, y] == null) {
					SpawnNewPiece (x, y, PieceType.EMPTY);
				}
			}
		}

		StartCoroutine(Fill ());
		AssignPowerUps ();
	}

	// Update is called once per frame
	void Update () {



		if(Input.GetKeyDown(KeyCode.H))
			ShuffleBoard();


		if (Input.GetKeyDown (KeyCode.L))
			PowerupElement ();
			
		if (Input.GetKeyDown (KeyCode.P))
			ClearRandomElements ();
	}

	private void AssignPowerUps()
	{

		for (int i = 0; i < PowerUpIcons.Length; i++) {

			if (CurrencySystem.PowerUps [i] == "Shuffle") {

				PowerUpIcons [i].GetComponent<Image> ().sprite = PowerUpAssets [0];
				PowerUpIcons [i].GetComponent<Button> ().onClick.AddListener (ShuffleBoard);

			} else if (CurrencySystem.PowerUps [i] == "Time") {

				PowerUpIcons [i].GetComponent<Image> ().sprite = PowerUpAssets [1];
				PowerUpIcons [i].GetComponent<Button> ().onClick.AddListener (TimePowerUp);
				
			} else if (CurrencySystem.PowerUps [i] == "PowerUp") {

				PowerUpIcons [i].GetComponent<Image> ().sprite = PowerUpAssets [2];
				PowerUpIcons [i].GetComponent<Button> ().onClick.AddListener (PowerupElement);
			}

		}

	}

	public void TurnOfPowerUp(int index)
	{
		PowerUpIcons [index].GetComponent<Image> ().sprite = NoGem;
		PowerUpIcons [index].GetComponent<Button> ().onClick.RemoveAllListeners ();

	}

	public void TimePowerUp()
	{
		level.GetComponent<LevelTimer> ().IncreaseTimer ();
	}

	public void ClearRandomElements()
	{
		List<GamePiece> ListOfPieces = new List<GamePiece> ();

		for (int i = 0; i < xDim; i++) {
			for (int j = 0; j < yDim; j++) {
				ListOfPieces.Add (pieces [i, j]);
			}
		}
			
		int Amount = 5;

		for (int i = 0; i < Amount; i++) {

			//Destroy (ListOfPieces [i].gameObject);

			int RandomValue = Random.Range (0, ListOfPieces.Count);
			//ListOfPieces[RandomValue].ClearableComponent.Clear ();
			Destroy (ListOfPieces [RandomValue].gameObject);
		}

		StartCoroutine (Fill ());
		FillStep();

	}

	public void ShuffleBoard()
	{

		List<GamePiece> ListOfPieces = new List<GamePiece> ();
		List<PiecePair> ListOfPairs = new List<PiecePair> ();

		for (int i = 0; i < xDim; i++) {
			for (int j = 0; j < yDim; j++) {
				ListOfPieces.Add (pieces [i, j]);
			}
		}
		for (int x = 0; x < ListOfPieces.Count; x++) {

			PiecePair newPair;
			newPair.A = ListOfPieces[x];
			ListOfPieces.Remove (ListOfPieces [x]);

			newPair.B = ListOfPieces [Random.Range (0, ListOfPieces.Count)];
			//Debug.Log (newPair.A.X + " " + newPair.A.Y + " gap " + newPair.B.X + " " + newPair.B.Y);
			ListOfPairs.Add (newPair);
		}
		Debug.Log(ListOfPairs.Count);

		for (int i = 0; i < ListOfPairs.Count; i++) {

			ColorPiece.ColorType typeA = ListOfPairs [i].A.ColorComponent.Color;
			ColorPiece.ColorType typeB = ListOfPairs [i].B.ColorComponent.Color;

			ListOfPairs [i].A.ColorComponent.Color = typeB;	
			ListOfPairs [i].B.ColorComponent.Color = typeA;

			lValidMatches ();

		}
		StartCoroutine(Fill ());

	}

	public IEnumerator Fill()
	{
		bool needsRefill = true;
		isFilling = true;

		while (needsRefill) {
			yield return new WaitForSeconds (fillTime);

			while (FillStep ()) {
				inverse = !inverse;
				yield return new WaitForSeconds (fillTime);
			}

			needsRefill = lValidMatches ();
		}

		isFilling = false;
	}

	public bool FillStep()
	{
		bool movedPiece = false;

		for (int y = yDim-2; y >= 0; y--)
		{
			for (int loopX = 0; loopX < xDim; loopX++)
			{
				int x = loopX;

				if (inverse) {
					x = xDim - 1 - loopX;
				}

				GamePiece piece = pieces [x, y];

				if (piece.IsMovable ())
				{
					GamePiece pieceBelow = pieces [x, y + 1];

					if (pieceBelow.Type == PieceType.EMPTY) {
						Destroy (pieceBelow.gameObject);
						piece.MovableComponent.Move (x, y + 1, fillTime);
						pieces [x, y + 1] = piece;
						SpawnNewPiece (x, y, PieceType.EMPTY);
						movedPiece = true;
					} else {
						for (int diag = -1; diag <= 1; diag++)
						{
							if (diag != 0)
							{
								int diagX = x + diag;

								if (inverse)
								{
									diagX = x - diag;
								}

								if (diagX >= 0 && diagX < xDim)
								{
									GamePiece diagonalPiece = pieces [diagX, y + 1];

									if (diagonalPiece.Type == PieceType.EMPTY)
									{
										bool hasPieceAbove = true;

										for (int aboveY = y; aboveY >= 0; aboveY--)
										{
											GamePiece pieceAbove = pieces [diagX, aboveY];

											if (pieceAbove.IsMovable ())
											{
												break;
											}
											else if(!pieceAbove.IsMovable() && pieceAbove.Type != PieceType.EMPTY)
											{
												hasPieceAbove = false;
												break;
											}
										}

										if (!hasPieceAbove)
										{
											Destroy (diagonalPiece.gameObject);
											piece.MovableComponent.Move (diagX, y + 1, fillTime);
											pieces [diagX, y + 1] = piece;
											SpawnNewPiece (x, y, PieceType.EMPTY);
											movedPiece = true;
											break;
										}
									} 
								}
							}
						}
					}
				}
			}
		}

		for (int x = 0; x < xDim; x++)
		{
			GamePiece pieceBelow = pieces [x, 0];

			if (pieceBelow.Type == PieceType.EMPTY)
			{
				Destroy (pieceBelow.gameObject);
				GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, -1), Quaternion.identity);
				newPiece.transform.parent = transform;

				pieces [x, 0] = newPiece.GetComponent<GamePiece> ();
				pieces [x, 0].Init (x, -1, this, PieceType.NORMAL);
				pieces [x, 0].MovableComponent.Move (x, 0, fillTime);
				pieces [x, 0].ColorComponent.SetColor ((ColorPiece.ColorType)Random.Range (0, pieces [x, 0].ColorComponent.NumColors));
				movedPiece = true;
			}
		}

		return movedPiece;
	}

	public Vector2 GetWorldPosition(int x, int y)
	{
		return new Vector2 (transform.position.x - xDim / 2.0f + x,
			transform.position.y + yDim / 2.0f - y);
	}

	public GamePiece SpawnNewPiece(int x, int y, PieceType type)
	{
		GameObject newPiece = (GameObject)Instantiate (piecePrefabDict [type], GetWorldPosition (x, y), Quaternion.identity);
		newPiece.transform.parent = transform;

		pieces [x, y] = newPiece.GetComponent<GamePiece> ();
		pieces [x, y].Init (x, y, this, type);

		return pieces [x, y];
	}

	public bool IsAdjacent(GamePiece piece1, GamePiece piece2)
	{
		return (piece1.X == piece2.X && (int)Mathf.Abs (piece1.Y - piece2.Y) == 1)
		|| (piece1.Y == piece2.Y && (int)Mathf.Abs (piece1.X - piece2.X) == 1);
	}

	public void SwapPieces(GamePiece piece1, GamePiece piece2)
	{
		if (gameOver) {
			return;
		}

		if (piece1.IsMovable () && piece2.IsMovable ()) {
			pieces [piece1.X, piece1.Y] = piece2;
			pieces [piece2.X, piece2.Y] = piece1;

			if (GetMatch (piece1, piece2.X, piece2.Y) != null || GetMatch (piece2, piece1.X, piece1.Y) != null
				|| piece1.Type == PieceType.RAINBOW || piece2.Type == PieceType.RAINBOW) {

				int piece1X = piece1.X;
				int piece1Y = piece1.Y;

				piece1.MovableComponent.Move (piece2.X, piece2.Y, fillTime);
				piece2.MovableComponent.Move (piece1X, piece1Y, fillTime);

				if (piece1.Type == PieceType.RAINBOW && piece1.IsClearable () && piece2.IsColored ()) {
					ClearColorPiece clearColor = piece1.GetComponent<ClearColorPiece> ();

					if (clearColor) {
						clearColor.Color = piece2.ColorComponent.Color;
					}

					ClearPiece (piece1.X, piece1.Y);
				}

				if (piece2.Type == PieceType.RAINBOW && piece2.IsClearable () && piece1.IsColored ()) {
					ClearColorPiece clearColor = piece2.GetComponent<ClearColorPiece> ();

					if (clearColor) {
						clearColor.Color = piece1.ColorComponent.Color;
					}

					ClearPiece (piece2.X, piece2.Y);
				}

				lValidMatches ();

				if (piece1.Type == PieceType.ROW_CLEAR || piece1.Type == PieceType.COLUMN_CLEAR) {
					ClearPiece (piece1.X, piece1.Y);
				}

				if (piece2.Type == PieceType.ROW_CLEAR || piece2.Type == PieceType.COLUMN_CLEAR) {
					ClearPiece (piece2.X, piece2.Y);
				}

				pressedPiece = null;
				enteredPiece = null;

				StartCoroutine (Fill ());

				level.OnMove ();
			} else {
				pieces [piece1.X, piece1.Y] = piece1;
				pieces [piece2.X, piece2.Y] = piece2;
				GetComponent<AudioSource> ().Play ();
				Handheld.Vibrate ();
			}
		}
	}

	public void PressPiece(GamePiece piece)
	{
		pressedPiece = piece;
		int xpos = piece.GetComponent<GamePiece> ().X;
		int ypos = piece.GetComponent<GamePiece> ().Y;

		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {

				pieces [i, j].gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			}
		}

		const int boxsize = 5;
		/*
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {

				if ((i == 0 || i == 2) && (j == 0 || j == 2) || i == 1 && j == 1)
					continue;

				if(i+xpos-1 >= 0 && i+xpos-1 <=7 && j+ypos-1 >= 0 && j+ypos-1 <=7)
				AdjacentObjects.Add (pieces [i+xpos-1, j+ypos-1].gameObject);

				if (i == 0 || i == 2 && AdjacentObjects.Count - 1 >= 0 && AdjacentObjects.Count - 1 <=8 && AdjacentObjects [AdjacentObjects.Count - 1] != null && AdjacentObjects.Count > 0) {
					
					AdjacentObjects [AdjacentObjects.Count - 1].gameObject.GetComponent<BoxCollider2D> ().size = new Vector2 (boxsize*5, boxsize);
					AdjacentObjects [AdjacentObjects.Count - 1].gameObject.GetComponent<BoxCollider2D> ().enabled = true;
					if(i==2)
					AdjacentObjects [AdjacentObjects.Count - 1].gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2 ((boxsize*5 / 2)-(boxsize/2), 0);
					if(i==0)
						AdjacentObjects [AdjacentObjects.Count - 1].gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2 (-((boxsize*5 / 2)-(boxsize/2)), 0);
				} else if(AdjacentObjects.Count - 1 >= 0 && AdjacentObjects.Count - 1 <=8){
					
					AdjacentObjects [AdjacentObjects.Count - 1].gameObject.GetComponent<BoxCollider2D> ().size = new Vector2 (boxsize, boxsize*5);
					AdjacentObjects [AdjacentObjects.Count - 1].gameObject.GetComponent<BoxCollider2D> ().enabled = true;
					if(j==2)
						AdjacentObjects [AdjacentObjects.Count - 1].gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2 (0, -(boxsize*5 / 2)+(boxsize/2));
					if(j==0)
						AdjacentObjects [AdjacentObjects.Count - 1].gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2 (0, ((boxsize*5 / 2)-(boxsize/2)));
				}


			}
		}
*/



		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {

				if ((i == 0 || i == 2) && (j == 0 || j == 2) || i == 1 && j == 1)
					continue;

				if(i+xpos-1 >= 0 && i+xpos-1 <=7 && j+ypos-1 >= 0 && j+ypos-1 <=7)
					//AdjacentObjects.Add (pieces [i+xpos-1, j+ypos-1].gameObject);

				if (i == 0 || i == 2) {

					pieces [i+xpos-1, j+ypos-1].gameObject.GetComponent<BoxCollider2D> ().size = new Vector2 (boxsize*5, boxsize);
					pieces [i+xpos-1, j+ypos-1].gameObject.GetComponent<BoxCollider2D> ().enabled = true;
					if(i==2)
						pieces [i+xpos-1, j+ypos-1].gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2 ((boxsize*5 / 2)-(boxsize/2), 0);
					if(i==0)
						pieces [i+xpos-1, j+ypos-1].gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2 (-((boxsize*5 / 2)-(boxsize/2)), 0);
				} else{

					pieces [i+xpos-1, j+ypos-1].gameObject.GetComponent<BoxCollider2D> ().size = new Vector2 (boxsize, boxsize*5);
					pieces [i+xpos-1, j+ypos-1].gameObject.GetComponent<BoxCollider2D> ().enabled = true;
					if(j==2)
						pieces [i+xpos-1, j+ypos-1].gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2 (0, -(boxsize*5 / 2)+(boxsize/2));
					if(j==0)
						pieces [i+xpos-1, j+ypos-1].gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2 (0, ((boxsize*5 / 2)-(boxsize/2)));
				}


			}
		}

		for (int i = 0; i < AdjacentObjects.Count; i++) {
			//AdjacentObjects.RemoveAt (0);
			//AdjacentObjects.Clear();
		}
		//AdjacentObjects.Clear();
	}

	public void EnterPiece(GamePiece piece)
	{
		enteredPiece = piece;
	}

	public void ReleasePiece()
	{

		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {

				pieces [i, j].gameObject.GetComponent<BoxCollider2D> ().enabled = true;
				pieces [i, j].gameObject.GetComponent<BoxCollider2D> ().size = Vector2.one * 5;
				pieces [i, j].gameObject.GetComponent<BoxCollider2D> ().offset = Vector2.zero;
			}
		}
		if (IsAdjacent (pressedPiece, enteredPiece)) {


			SwapPieces (pressedPiece, enteredPiece);
		} 

	}

	public List<GamePiece> GetMatch(GamePiece piece, int newX, int newY)
	{
		if (piece.IsColored ()) {
			ColorPiece.ColorType color = piece.ColorComponent.Color;
			List<GamePiece> horizontalPieces = new List<GamePiece> ();
			List<GamePiece> verticalPieces = new List<GamePiece> ();
			List<GamePiece> matchingPieces = new List<GamePiece> ();

			// First check horizontal
			horizontalPieces.Add(piece);

			for (int dir = 0; dir <= 1; dir++) {
				for (int xOffset = 1; xOffset < xDim; xOffset++) {
					int x;

					if (dir == 0) { // Left
						x = newX - xOffset;
					} else { // Right
						x = newX + xOffset;
					}

					if (x < 0 || x >= xDim) {
						break;
					}

					if (pieces [x, newY].IsColored () && pieces [x, newY].ColorComponent.Color == color) {
						horizontalPieces.Add (pieces [x, newY]);
					} else {
						break;
					}
				}
			}

			if (horizontalPieces.Count >= 3) {
				for (int i = 0; i < horizontalPieces.Count; i++) {
					matchingPieces.Add (horizontalPieces [i]);
				}
			}

			// Traverse vertically if we found a match (for L and T shapes)
			if (horizontalPieces.Count >= 3) {
				for (int i = 0; i < horizontalPieces.Count; i++) {
					for (int dir = 0; dir <= 1; dir++) {
						for (int yOffset = 1; yOffset < yDim; yOffset++) {
							int y;

							if (dir == 0) { // Up
								y = newY - yOffset;
							} else { // Down
								y = newY + yOffset;
							}

							if (y < 0 || y >= yDim) {
								break;
							}

							if (pieces [horizontalPieces [i].X, y].IsColored () && pieces [horizontalPieces [i].X, y].ColorComponent.Color == color) {
								verticalPieces.Add (pieces [horizontalPieces [i].X, y]);
							} else {
								break;
							}
						}
					}

					if (verticalPieces.Count < 2) {
						verticalPieces.Clear ();
					} else {
						for (int j = 0; j < verticalPieces.Count; j++) {
							matchingPieces.Add (verticalPieces [j]);
						}

						break;
					}
				}
			}

			if (matchingPieces.Count >= 3) {
				return matchingPieces;
			}

			// Didn't find anything going horizontally first,
			// so now check vertically
			horizontalPieces.Clear();
			verticalPieces.Clear ();
			verticalPieces.Add(piece);

			for (int dir = 0; dir <= 1; dir++) {
				for (int yOffset = 1; yOffset < yDim; yOffset++) {
					int y;

					if (dir == 0) { // Up
						y = newY - yOffset;
					} else { // Down
						y = newY + yOffset;
					}

					if (y < 0 || y >= yDim) {
						break;
					}

					if (pieces [newX, y].IsColored () && pieces [newX, y].ColorComponent.Color == color) {
						verticalPieces.Add (pieces [newX, y]);
					} else {
						break;
					}
				}
			}

			if (verticalPieces.Count >= 3) {
				for (int i = 0; i < verticalPieces.Count; i++) {
					matchingPieces.Add (verticalPieces [i]);
				}
			}

			// Traverse horizontally if we found a match (for L and T shapes)
			if (verticalPieces.Count >= 3) {
				for (int i = 0; i < verticalPieces.Count; i++) {
					for (int dir = 0; dir <= 1; dir++) {
						for (int xOffset = 1; xOffset < xDim; xOffset++) {
							int x;

							if (dir == 0) { // Left
								x = newX - xOffset;
							} else { // Right
								x = newX + xOffset;
							}

							if (x < 0 || x >= xDim) {
								break;
							}

							if (pieces [x, verticalPieces[i].Y].IsColored () && pieces [x, verticalPieces[i].Y].ColorComponent.Color == color) {
								horizontalPieces.Add (pieces [x, verticalPieces[i].Y]);
							} else {
								break;
							}
						}
					}

					if (horizontalPieces.Count < 2) {
						horizontalPieces.Clear ();
					} else {
						for (int j = 0; j < horizontalPieces.Count; j++) {
							matchingPieces.Add (horizontalPieces [j]);
						}

						break;
					}
				}
			}

			if (matchingPieces.Count >= 3) {
				return matchingPieces;
			}
		}

		return null;
	}

	public bool lValidMatches()
	{
		bool needsRefill = false;

		for (int y = 0; y < yDim; y++) {
			for (int x = 0; x < xDim; x++) {
				if (pieces [x, y].IsClearable ()) {
					List<GamePiece> match = GetMatch (pieces [x, y], x, y);


						

					if (match != null) {
						PieceType specialPieceType = PieceType.COUNT;
						GamePiece randomPiece = match [Random.Range (0, match.Count)];
						int specialPieceX = randomPiece.X;
						int specialPieceY = randomPiece.Y;

						if (match.Count == 4) {
							if (pressedPiece == null || enteredPiece == null) {
								specialPieceType = (PieceType)Random.Range ((int)PieceType.ROW_CLEAR, (int)PieceType.COLUMN_CLEAR);
							} else if (pressedPiece.Y == enteredPiece.Y) {
								specialPieceType = PieceType.ROW_CLEAR;
							} else {
								specialPieceType = PieceType.COLUMN_CLEAR;
							}
						} else if (match.Count >= 5) {
							specialPieceType = PieceType.RAINBOW;
						}

						for (int i = 0; i < match.Count; i++) {
							if (ClearPiece (match [i].X, match [i].Y)) {
								needsRefill = true;

								if (match [i] == pressedPiece || match [i] == enteredPiece) {
									specialPieceX = match [i].X;
									specialPieceY = match [i].Y;
								}
							}
						}

						if (specialPieceType != PieceType.COUNT) {
							Destroy (pieces [specialPieceX, specialPieceY]);
							GamePiece newPiece = SpawnNewPiece (specialPieceX, specialPieceY, specialPieceType);

							if ((specialPieceType == PieceType.ROW_CLEAR || specialPieceType == PieceType.COLUMN_CLEAR)
							    && newPiece.IsColored () && match [0].IsColored ()) {
								newPiece.ColorComponent.SetColor (match [0].ColorComponent.Color);
							} else if (specialPieceType == PieceType.RAINBOW && newPiece.IsColored ()) {
								newPiece.ColorComponent.SetColor (ColorPiece.ColorType.ANY);
							}
						}
					}
				}
			}
		}

		return needsRefill;
	}

	public void PowerupElement()
	{
		int x = Random.Range (0, 8);
		int y = Random.Range (0, 8);

		ColorPiece.ColorType currentType = pieces [x, y].ColorComponent.Color;
		Destroy(pieces[x,y].gameObject);

		GamePiece test = SpawnNewPiece (x, y, PieceType.COLUMN_CLEAR);
		test.ColorComponent.SetColor (currentType);
		//pieces[1,1].

	}



	public bool ClearPiece(int x, int y)
	{
		
		if (pieces [x, y].IsClearable () && !pieces [x, y].ClearableComponent.IsBeingCleared) {
			pieces [x, y].ClearableComponent.Clear ();
			SpawnNewPiece (x, y, PieceType.EMPTY);

			ClearObstacles (x, y);



			return true;
		}

		return false;
	}

	public void ClearObstacles(int x, int y)
	{
		for (int adjacentX = x - 1; adjacentX <= x + 1; adjacentX++) {
			if (adjacentX != x && adjacentX >= 0 && adjacentX < xDim) {
				if (pieces [adjacentX, y].Type == PieceType.BUBBLE && pieces [adjacentX, y].IsClearable ()) {
					pieces [adjacentX, y].ClearableComponent.Clear ();
					SpawnNewPiece (adjacentX, y, PieceType.EMPTY);
				}
			}
		}

		for (int adjacentY = y - 1; adjacentY <= y + 1; adjacentY++) {
			if (adjacentY != y && adjacentY >= 0 && adjacentY < yDim) {
				if (pieces [x, adjacentY].Type == PieceType.BUBBLE && pieces [x, adjacentY].IsClearable ()) {
					pieces [x, adjacentY].ClearableComponent.Clear ();
					SpawnNewPiece (x, adjacentY, PieceType.EMPTY);
				}
			}
		}
	}

	public void ClearRow(int row)
	{
		for (int x = 0; x < xDim; x++) {
			ClearPiece (x, row);
		}


	}

	public void ClearColumn(int column)
	{
		for (int y = 0; y < yDim; y++) {
			ClearPiece (column, y);
		}


	}

	public void ClearColor(ColorPiece.ColorType color, GameObject LightningLocation)
	{
		
		for (int x = 0; x < xDim; x++) {
			for (int y = 0; y < yDim; y++) {
				if (pieces [x, y].IsColored () && (pieces [x, y].ColorComponent.Color == color
				    || color == ColorPiece.ColorType.ANY)) {
					GameObject Lightning = Instantiate (LightningObject);
					LightningObject.GetComponent<Electric> ().transformPointA = LightningLocation.transform.position;
					LightningObject.GetComponent<Electric> ().transformPointB = pieces [x, y].gameObject.transform.position;
					ClearPiece (x, y);
				}
			}
		}
	}

	public void GameOver()
	{
		gameOver = true;
	}

	public List<GamePiece> GetPiecesOfType(PieceType type)
	{
		List<GamePiece> piecesOfType = new List<GamePiece> ();

		for (int x = 0; x < xDim; x++) {
			for (int y = 0; y < yDim; y++) {
				if (pieces [x, y].Type == type) {
					piecesOfType.Add (pieces [x, y]);
				}
			}
		}

		return piecesOfType;
	}
}
