using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCamera : MonoBehaviour {

	void OnCollisionEnter(Collision col)
	{
		if (col.transform.tag == "Player") 
		{
			Camera.main.GetComponent<IsometricCamera> ().enabled = true;
			Camera.main.GetComponent<AsteroidCamera> ().enabled = false;
		}
	}
}
