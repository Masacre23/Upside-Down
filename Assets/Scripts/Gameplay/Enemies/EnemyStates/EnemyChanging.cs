using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChanging : EnemyStates {

	// Use this for initialization
	public override void Start () {
		base.Start ();
		m_type = States.CHANGING;
	}
	
	// Update is called once per frame
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
