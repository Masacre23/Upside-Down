using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCamera : MonoBehaviour {

	public GameObject player;
	public GameObject[] lerpPoints;
	public int actualPoint;
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (player.transform, player.transform.up);
		if(actualPoint != -1 && transform.parent == null)
			transform.position = Vector3.Lerp (transform.position, lerpPoints [actualPoint].transform.position, Time.deltaTime);
	}
}
