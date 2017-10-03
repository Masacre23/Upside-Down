using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAsteroidCam : MonoBehaviour {

	void OnCollisionEnter(Collision col)
	{
		if (col.transform.tag == "Player")
			Camera.main.GetComponent<AsteroidCamera> ().reference = transform;
	}
}
