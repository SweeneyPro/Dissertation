﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicHandler : MonoBehaviour {

	[SerializeField]
	private AudioClip MenuMusic;

	[SerializeField]
	private AudioClip InGameMusic;

	private static MusicHandler instance = null;

	void Awake()
	{

		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(this.gameObject);
			UnityEngine.SceneManagement.SceneManager.sceneLoaded += SilenceObject;
			if (GetComponent<AudioSource> ().clip == null) {
				GetComponent<AudioSource> ().clip = MenuMusic;
				GetComponent<AudioSource> ().Play ();
			}
		}
		else if(instance != this){


		} 
			

	}


	
	// Update is called once per frame
	void Update () {
		//print(GetComponent<AudioSource> ().time);
		//print(GetComponent<AudioSource> ().clip.name);
	}

	void SilenceObject(Scene scene, LoadSceneMode mode)
	{
		
		int temp;
		if (int.TryParse(scene.name, out temp) || scene.name == "Blitz" && GetComponent<AudioSource> ().clip != InGameMusic) {

			GetComponent<AudioSource> ().clip = InGameMusic;
			GetComponent<AudioSource> ().Play ();
		} else if (GetComponent<AudioSource> ().clip != MenuMusic){

			GetComponent<AudioSource> ().clip = MenuMusic;
			GetComponent<AudioSource> ().Play ();
		}
	}
}
