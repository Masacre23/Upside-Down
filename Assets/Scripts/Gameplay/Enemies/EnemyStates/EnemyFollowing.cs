using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowing : EnemyStates {
	public GameObject player;
	int speed;
	public bool canChange = true;
	//public GameObject wallToChange;
	public Vector3 up;
	float radiusCollider;
	public float rotationX;
	public float rotationY;
	public float rotationZ;

	// Use this for initialization
	public override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public override bool OnUpdate () {
		up = transform.up;
		bool ret = false;

		/*if (player != null) 
		{
			if (canChange)
				Move ();
			else if (player.transform.up.y >= transform.up.y - 0.1f && player.transform.up.y <= transform.up.y + 0.1f)
				Move ();
			/*else if (wallToChange != null) 
			{
			}*/
		//}
		Move();
		return ret;
	}

	public override void OnEnter()
	{
		m_type = States.FOLLOWING;

		player = m_enemy.player;
		speed = m_enemy.m_speed;

		radiusCollider = m_enemy.GetComponent<SphereCollider> ().radius;
		m_enemy.GetComponent<SphereCollider> ().radius = 0;
	}

	public override void OnExit()
	{
		m_enemy.GetComponent<SphereCollider> ().radius = radiusCollider;
	}

	public void Move()
	{
		Vector3 target = player.transform.position;

		Vector3 difference = target - transform.position;

		float distanceToPlane = Vector3.Dot(transform.up, target - transform.position);
		Vector3 pointOnPlane = target - (transform.up * distanceToPlane);

		transform.LookAt(pointOnPlane, transform.up);
	
		transform.position += transform.forward * speed * Time.deltaTime;
	}
}
