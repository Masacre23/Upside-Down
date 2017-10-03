using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStunned : BossStates {

	public override void Start () 
	{
		base.Start ();
		m_type = States.STUNNED;
	}
	
	// Update is called once per frame
	public override bool OnUpdate () 
	{
		bool ret = false;

		return ret;
	}

	public override void OnEnter()
	{
		m_boss.m_animator.SetBool ("IsStunned", true);
	}

	public override void OnExit()
	{
		m_boss.m_animator.SetBool ("IsStunned", false);
	}
}
