using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdIdle : BirdStates {

	int damp = 2;

	public override void Start ()
	{
		base.Start ();
		m_type = States.IDLE;
	}
	
	//Main enemy update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
	public override bool OnUpdate (DamageData data)
	{
		bool ret = false;

		if (data.m_recive)
		{
			ret = true;
			m_bird.DamageManager(data);
		}

		if (m_bird.player)
		{
			ret = true;
			m_bird.m_currentState = m_bird.m_Attacking;
			m_bird.m_Attacking.OnEnter ();
		}

		Fly ();

		return ret;
	}

	void Fly()
	{
		Quaternion rotationAngle = Quaternion.LookRotation(transform.forward + transform.right, transform.up);
		Quaternion temp = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * damp);
		transform.rotation = new Quaternion(transform.rotation.x, temp.y, transform.rotation.z, temp.w);
		transform.position += transform.forward * m_bird.m_moveSpeed * Time.deltaTime;
	}

	public override void OnEnter()
	{
		/*if (m_enemy.m_animator != null)
		{
			m_enemy.m_animator.SetBool("PlayerDetected", false);
		}*/
	}

	public override void OnExit()
	{

	}
}
