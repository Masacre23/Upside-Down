using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdle : BossStates {
	float counter;

	public override void Start ()
	{
		base.Start ();
		m_type = States.IDLE;
	}
	
	// Update is called once per frame
	public override bool OnUpdate () 
	{
		bool ret = false;

		counter += Time.deltaTime;
		if (counter >= m_boss.m_attackRate) 
		{
			ret = true;
			counter = 0;
			m_boss.m_currentState = m_boss.m_Attack;
		}

		return ret;
	}

	public override void OnEnter()
	{
	}

	public override void OnExit()
	{
	}
}
