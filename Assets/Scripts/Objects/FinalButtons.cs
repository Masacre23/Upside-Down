using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalButtons : MonoBehaviour {
	[SerializeField] GameObject finalDoor;
	bool pressed = false;

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "GravityAffected" && !pressed) {
			finalDoor.GetComponent<FinalDoor> ().Activate ();
			pressed = true;
		}
	}
}
