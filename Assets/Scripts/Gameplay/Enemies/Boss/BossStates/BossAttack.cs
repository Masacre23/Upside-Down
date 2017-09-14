using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : BossStates {

	public override void Start ()
	{
		base.Start ();
		m_type = States.ATTACK;
	}

	// Update is called once per frame
	public override bool OnUpdate () 
	{
		bool ret = false;

		if (m_boss.m_phase == 2)
			Move ();
		
		AnimatorStateInfo info = m_boss.m_animator.GetCurrentAnimatorStateInfo (0);
		if (!info.IsName ("Attack") && !info.IsName ("Attack2")) 
		{
			ret = true;
			m_boss.m_currentState = m_boss.m_Idle;
		}

		return ret;
	}

	void Move()
	{
		//m_rigidBody.isKinematic = true;
		Quaternion rotation = Quaternion.LookRotation(m_boss.player.transform.position - transform.position, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_boss.m_rotationSpeed * 16);

		Vector3 heading = m_boss.player.transform.position - transform.position;
		heading.y = 0;
		float distance = heading.magnitude;
		Vector3 direction = heading / distance;

		transform.position += direction * Time.deltaTime * m_boss.m_speed;
	}

	public override void OnEnter()
	{
		m_boss.m_animator.SetBool ("IsAttacking", true);
	}

	public override void OnExit()
	{
		m_boss.m_animator.SetBool ("IsAttacking", false);
	}
}
