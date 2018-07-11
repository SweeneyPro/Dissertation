using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

	public float Delay;

	// Use this for initialization
	void Start()
	{
		StartCoroutine(coroutineA());
	}

	IEnumerator coroutineA()
	{
		yield return new WaitForSeconds(Delay);
		Destroy (gameObject);
	}
}
