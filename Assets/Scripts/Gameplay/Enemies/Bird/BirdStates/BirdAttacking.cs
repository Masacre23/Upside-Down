using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAttacking : BirdStates {
	Vector3 origPos;
	bool down = true;
	float origTime;
	float time;
	int state = 0;

	public override void Start ()
	{
		base.Start ();
		m_type = States.ATTACKING;
		origPos = transform.position;
		origTime = Time.time;
		state = 0;
	}
	
	//Main enemy update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
	public override bool OnUpdate (DamageData data)
	{
		Attack ();

	/*	if (!down && transform.position == origPos) 
		{
			m_bird.m_currentState = m_bird.m_Idle;
			down = true;
		}*/

		return true;
	}

	void Attack()
	{
		if (time > 1) 
		{
			state++;
			time = 0;
		}
		time += Time.deltaTime;
		switch (state) 
		{
		case 0:
			transform.position -= transform.up / 25;
			break;
		case 1:
			break;
		case 2:
			transform.position += transform.up / 25;
			break;
		case 3:
			m_bird.m_currentState = m_bird.m_Idle;
			state = 0;
			break;
		}

	}

	public override void OnEnter()
	{
	}

	public override void OnExit()
	{
		m_bird.player = null;
	}
}
