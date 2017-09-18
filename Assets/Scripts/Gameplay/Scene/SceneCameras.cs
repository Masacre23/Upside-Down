using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCameras : MonoBehaviour {
	float timeCamera;
	int numCamera;
	public float changeTime;

	// Use this for initialization
	void Start () {
		numCamera = 0;
		transform.GetChild (0).gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		timeCamera += Time.deltaTime;
		if (timeCamera > changeTime) 
		{
			timeCamera = 0;
			transform.GetChild (numCamera).gameObject.SetActive (false);
			numCamera++;
			if (numCamera < transform.childCount)
				transform.GetChild (numCamera).gameObject.SetActive (true);
			else 
			{
				transform.GetChild (numCamera - 1).gameObject.SetActive (false);
				Start ();
			}
		}
	}
}
