using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (GameObject.Find ("Player").transform.position, GameObject.Find ("Player").transform.up);

		/*Vector3 target = GameObject.Find ("Player").transform.position;

		Vector3 difference = target - transform.position;

		float distanceToPlane = Vector3.Dot(transform.up, difference);
		Vector3 pointOnPlane = target - (transform.up * distanceToPlane);

		//Original
		transform.LookAt(pointOnPlane, transform.up);*/
	}
}
