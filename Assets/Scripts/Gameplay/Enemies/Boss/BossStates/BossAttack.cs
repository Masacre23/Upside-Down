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

		if (!m_boss.m_animator.GetCurrentAnimatorStateInfo (0).IsName ("Attack")) 
		{
			ret = true;
			m_boss.m_currentState = m_boss.m_Idle;
		}

		return ret;
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
