using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFaceCamera : MonoBehaviour {

	public int numPoint = -1;
	public Vector3 dir;

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
			Camera.main.transform.parent = null;
			Camera.main.GetComponent<IsometricCamera> ().actualPoint = numPoint;
			Camera.main.GetComponent<IsometricCamera> ().dir = Vector3.zero;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player") 
		{
			if (dir == Vector3.zero)
				Camera.main.transform.parent = GameObject.Find ("Player").transform;
			else 
			{
				Camera.main.transform.parent = null;
				Camera.main.GetComponent<IsometricCamera> ().dir = dir;
			}
		}
	}
}
