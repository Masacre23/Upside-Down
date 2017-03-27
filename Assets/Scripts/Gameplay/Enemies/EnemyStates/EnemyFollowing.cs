using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowing : EnemyStates {
	public GameObject player;
	int speed;
	public bool canChange = true;
	public GameObject wallToChange;
	public Vector3 up;
	float radiusCollider;
	public bool x;
	public bool y;
	public bool z;
	public Vector3 bUp; //a boolean vector
	public float rotationX;
	public float rotationY;
	public float rotationZ;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		x = false;
		y = false;
		z = false;
	}
	
	// Update is called once per frame
	public override bool OnUpdate () {
		up = transform.up;
		bool ret = false;

		if (player != null) 
		{
			if (canChange)
				Move ();
			else if (player.transform.up.y >= transform.up.y - 0.1f && player.transform.up.y <= transform.up.y + 0.1f)
				Move ();
			else if (wallToChange != null) 
			{
			}
		}
		return ret;
	}

	public override void OnEnter()
	{
		m_type = States.FOLLOWING;

		player = m_enemy.player;
		speed = m_enemy.m_speed;

		//m_rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		/*if (player.transform.up.x > 0.5f)
			m_rigidBody.constraints = RigidbodyConstraints.FreezeRotationX;
		if (player.transform.up.y)
			m_rigidBody.constraints |= RigidbodyConstraints.FreezeRotationY;
		if (player.transform.up.z )
			m_rigidBody.constraints |= RigidbodyConstraints.FreezeRotationZ;*/
		if (player.transform.up.x > 0.5f || player.transform.up.x < -0.5f)
			x = true;
		if (player.transform.up.y > 0.5f || player.transform.up.y < -0.5f)
			y = true;
		if (player.transform.up.z > 0.5f || player.transform.up.z < -0.5f)
			z = true;

		bUp = new Vector3 (transform.up.x , transform.up.y, transform.up.z);

		radiusCollider = m_enemy.GetComponent<SphereCollider> ().radius;
		m_enemy.GetComponent<SphereCollider> ().radius = 0;
	}

	public override void OnExit()
	{
		m_enemy.GetComponent<SphereCollider> ().radius = radiusCollider;
		x = false;
		y = false;
		z = false;
	}

	public void Move()
	{
		Vector3 target = player.transform.position;
		/*if (x)
		  	target.x = transform.position.x;
		if (y)
			target.y = transform.position.y;
		if (z)
			target.z = transform.position.z;*/
		//transform.LookAt (target);

		Vector3 difference = target - transform.position;
		/*if (x || z) 
		{
			float rotationX = Mathf.Atan2 (difference.y, difference.z) * Mathf.Rad2Deg;
			//if (transform.up.x < 0)
			rotationX *= -1;
			transform.rotation = Quaternion.Euler (rotationX , m_enemy.currentWall.transform.rotation.eulerAngles.y, m_enemy.currentWall.transform.rotation.eulerAngles.z);
		}
		else if (y || z) 
		{
			float rotationY = Mathf.Atan2 (difference.x, difference.z) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (m_enemy.currentWall.transform.rotation.eulerAngles.x, rotationY, m_enemy.currentWall.transform.rotation.eulerAngles.z);
		}
		else if (z) 
		{
			float rotationZ = Mathf.Atan2 (difference.y, difference.z) * Mathf.Rad2Deg;
			//if (transform.up.x < 0)
			//	rotationZ *= -1;
			rotationZ *= -1;
			transform.rotation = Quaternion.Euler (m_enemy.currentWall.transform.rotation.eulerAngles.x, m_enemy.currentWall.transform.rotation.eulerAngles.y, rotationZ);
		}*/



		/*rotationX = Mathf.Atan2 (difference.y, difference.z) * Mathf.Rad2Deg;
		rotationY = Mathf.Atan2 (difference.x, difference.z) * Mathf.Rad2Deg;
		rotationZ = Mathf.Atan2 (difference.x, difference.y) * Mathf.Rad2Deg;
	
		transform.rotation = Quaternion.Euler (rotationX * transform.up.x, 
			rotationY *transform.up.y, 
			rotationZ *transform.up.z);*/
		
		/*Vector3 lookPos = target - transform.position;
		if (x) {
			//lookPos.x = transform.position.x;
			lookPos.x = 0;
			//lookPos.z = transform.position.z;
		} else if (y)
			lookPos.y = 0;
		else if (z)
			lookPos.z = 0;
		Quaternion rotation = Quaternion.LookRotation (lookPos);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime);*/

		/*Vector3 targetPostition;
		if (x) {
			targetPostition = new Vector3 (this.transform.position.x, 
				                          target.y, 
				                          target.z);
		} else {
			targetPostition = new Vector3 (target.x, 
				this.transform.position.y, 
				target.z);
		}
		this.transform.LookAt( targetPostition ) ;*/

		float distanceToPlane = Vector3.Dot(transform.up, target - transform.position);
		Vector3 pointOnPlane = target - (transform.up * distanceToPlane);

		transform.LookAt(pointOnPlane, transform.up);

		transform.position += transform.forward * speed * Time.deltaTime;
	}
}
