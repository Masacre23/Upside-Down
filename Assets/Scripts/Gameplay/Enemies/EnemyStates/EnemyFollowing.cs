using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowing : EnemyStates {
	public GameObject player;
	int speed;
	public bool canChange;
	GameObject wallToChange;
	public Vector3 up;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		m_type = States.FOLLOWING;
		m_rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
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
		player = m_enemy.player;
		speed = m_enemy.m_speed;

		m_rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
	}

	public override void OnExit()
	{

	}

	public void Move()
	{
		Vector3 target = player.transform.position;
		target.y = transform.position.y;
		transform.LookAt (target);

		transform.position += transform.forward * speed * Time.deltaTime;
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "EnemyWall")
			wallToChange = col.gameObject;
	}
}
