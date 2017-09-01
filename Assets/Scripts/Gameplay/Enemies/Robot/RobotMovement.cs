using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovement : MonoBehaviour {
	public Vector3 direction;
	bool returning = false;
	public float distance;
	Vector3 startPoint;
	Vector3 endPoint;
	// Use this for initialization
	void Start () {
		startPoint = transform.position;
		endPoint = transform.position + direction * distance;
	}
	
	// Update is called once per frame
	void Update () {
		//float distance;
		if (!returning) 
		{
			distance = Vector3.Distance (transform.position, endPoint);
			transform.position += direction * distance * Time.deltaTime/10;
		} else 
		{
			distance = Vector3.Distance (transform.position, startPoint);
			transform.position += -direction * distance * Time.deltaTime/10;
		}

		if (distance < 0.5)
			returning = !returning;
	}
}
