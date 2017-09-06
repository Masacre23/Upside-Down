using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovement : MonoBehaviour {
	
	public enum movement
	{
		LINEAR,
		CIRCLES,
		TURRET
	}
	public movement moveType;
	[Header("LINEAR MOVEMENT")]
	public float acceleration = 0.01f;
	public float deceleration = 0.02f;
	public float maxSpeed = 0.01f;
	float speed;
	public Vector3 direction;
	bool returning = false;
	public float distance;
	Vector3 startPoint;
	Vector3 endPoint;

	[Header("CIRCLES & TURRET MOVEMENT")]
	public float rotationSpeed = 4.0f;
	public float damp = 2.0f;

	GameObject target;

	// Use this for initialization
	void Start () {
		startPoint = transform.position;
		endPoint = transform.position + direction * distance;
		target = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
		switch(moveType)
		{
		case movement.LINEAR:
			if (!returning && speed < maxSpeed) 
			{
				distance = Vector3.Distance (transform.position, endPoint);
				speed += acceleration * Time.deltaTime;
			//transform.position = Vector3.Lerp(transform.position, endPoint, speed);
			//transform.position += direction * distance * Time.deltaTime/10;
			} else if (speed > -maxSpeed) 
			{
				distance = Vector3.Distance (transform.position, startPoint);
				speed -= deceleration * Time.deltaTime;
			//transform.position = Vector3.Lerp (transform.position, startPoint, speed);
			//transform.position += -direction * distance * Time.deltaTime/10;
			} else 
			{
				if (speed > acceleration * Time.deltaTime)
					speed += -acceleration * Time.deltaTime;
				else if (speed < -deceleration * Time.deltaTime)
					speed += deceleration * Time.deltaTime;
				else
					speed = 0;
			}
			
			transform.position += direction * speed;
		
			if (distance < 1)
				returning = !returning;
			break;
		case movement.CIRCLES:
			//Quaternion rotationAngle = Quaternion.LookRotation (transform.forward + transform.right, transform.up);
			Quaternion rotationAngle = Quaternion.LookRotation (transform.forward, transform.right);
			Quaternion temp = Quaternion.Slerp (transform.rotation, rotationAngle, Time.deltaTime * damp);
			transform.rotation = new Quaternion (temp.x, temp.y, temp.z,  temp.w);
			//transform.position += transform.forward * rotationSpeed * Time.deltaTime;
			transform.position += transform.right * rotationSpeed * Time.deltaTime;
			break;
		case movement.TURRET:
			Quaternion rotation = Quaternion.LookRotation(target.transform.position - transform.position + target.transform.up/2);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
			break;
		}
	}

	void OnTriggerStay(Collider col)
	{
		if (col.tag == "Player") 
		{
			col.gameObject.GetComponent<Player> ().m_damageData.m_recive = true;
			col.gameObject.GetComponent<Player> ().m_damageData.m_damage = 20;
		}
	}
}
