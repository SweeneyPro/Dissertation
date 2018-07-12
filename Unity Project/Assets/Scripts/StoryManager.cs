using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour {

	[SerializeField]
	private GameObject[] ListOfLevels;

	// Use this for initialization
	void Start () {
		

		ActivateLevels ();

	}

	
	// Update is called once per frame
	void Update () {
		
	}



	void ActivateLevels()
	{

		for (int i = 0; i < ListOfLevels.Length; i++) {

			if (StoryProgress.score [i] == 0 && i != 0) {

				ListOfLevels [i].SetActive (false);

			} 

			if (i - 1 >= 0 && StoryProgress.score [i - 1] > 0) {

				ListOfLevels [i].SetActive (true);

			} else {

				// FILL THIS IN //////////////////////// 
			}

			if (StoryProgress.LevelStars [i] == 3) {
				
				ListOfLevels [i].transform.GetChild (1).gameObject.SetActive (true);
				ListOfLevels [i].transform.GetChild (2).gameObject.SetActive (true);
				ListOfLevels [i].transform.GetChild (3).gameObject.SetActive (true);
			} else if (StoryProgress.LevelStars [i] == 2) {

				ListOfLevels [i].transform.GetChild (1).gameObject.SetActive (true);
				ListOfLevels [i].transform.GetChild (2).gameObject.SetActive (true);
			} else if (StoryProgress.LevelStars [i] == 1) {
				ListOfLevels [i].transform.GetChild (1).gameObject.SetActive (true);
			}
				

		}
	}
}
