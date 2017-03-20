using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowing : EnemyStates {
	public GameObject player;
	int speed;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		m_type = States.FOLLOWING;
	}
	
	// Update is called once per frame
	public override bool OnUpdate () {
		bool ret = false;

		if (player != null)
			Move ();

		return ret;
	}

	public override void OnEnter()
	{
		player = m_enemy.player;
		speed = m_enemy.m_speed;
	}

	public override void OnExit()
	{

	}

	public void Move()
	{
		transform.LookAt (player.transform);

		transform.position += transform.forward * speed * Time.deltaTime;
	}
}
