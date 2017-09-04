using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovement : MonoBehaviour {
	public Vector3 direction;
	bool returning = false;
	public float distance;
	Vector3 startPoint;
	Vector3 endPoint;
	float speed;
	public float acc;
	public float maxSpeed = 1;

	// Use this for initialization
	void Start () {
		startPoint = transform.position;
		endPoint = transform.position + direction * distance;
	}
	
	// Update is called once per frame
	void Update () {
		//float distance;
		if (!returning && speed < maxSpeed) 
		{
			distance = Vector3.Distance (transform.position, endPoint);
			speed += Time.deltaTime / 100;
			//transform.position = Vector3.Lerp(transform.position, endPoint, speed);
			//transform.position += direction * distance * Time.deltaTime/10;
		} else if (speed > -maxSpeed) 
		{
			distance = Vector3.Distance (transform.position, startPoint);
			speed -= Time.deltaTime / 100;
			//transform.position = Vector3.Lerp (transform.position, startPoint, speed);
			//transform.position += -direction * distance * Time.deltaTime/10;
		} else 
		{
			if (speed > Time.deltaTime / 100)
				speed += -Time.deltaTime / 100;
			else if (speed < -Time.deltaTime / 100)
				speed += Time.deltaTime / 100;
			else
				speed = 0;
		}
			
		transform.position += direction * speed;
		/*if (!returning)
			transform.position = Vector3.Lerp(transform.position, endPoint, speed);
		else
			transform.position = Vector3.Lerp(transform.position, startPoint, speed);*/
		
		if (distance < 1)
			returning = !returning;
	}
}
