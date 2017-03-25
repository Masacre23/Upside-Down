using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : EnemyStates {

	public override void Start () {
		base.Start ();
		m_type = States.IDLE;
	}
	
	//Main enemy update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
	public override bool OnUpdate () {
		bool ret = false;

		return ret;
	}

	public override void OnEnter()
	{
		
	}

	public override void OnExit()
	{
		
	}
}
