using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFaceCamera : MonoBehaviour {

	public int numPoint = -1;

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
			Camera.main.transform.parent = null;
			//if (Camera.main.GetComponent<IsometricCamera> ().actualPoint != numPoint)
				Camera.main.GetComponent<IsometricCamera> ().actualPoint = numPoint;
			//else
			//	Camera.main.GetComponent<IsometricCamera> ().actualPoint--;
		}

	}

	void OnTriggerExit(Collider col)
	{
		if(col.tag == "Player")
			Camera.main.transform.parent = GameObject.Find ("Player").transform;
	}
}
