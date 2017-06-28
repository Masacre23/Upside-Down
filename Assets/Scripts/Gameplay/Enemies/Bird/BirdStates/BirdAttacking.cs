using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAttacking : BirdStates {

	public override void Start ()
	{
		base.Start ();
		m_type = States.ATTACKING;
	}
	
	//Main enemy update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
	public override bool OnUpdate (DamageData data)
	{
		return true;
	}

	public override void OnEnter()
	{
	}

	public override void OnExit()
	{

	}
}
