using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStates : MonoBehaviour {

	protected enum States
	{
		IDLE,
		ATTACK,
		STUNNED,
		DEAD
	}

	protected States m_type;
	protected Boss m_boss;
	protected Rigidbody m_rigidBody;

	// Use this for initialization
	public virtual void Start () {
		m_boss = GetComponent<Boss> ();
		m_rigidBody = GetComponent<Rigidbody> ();
	}
	
	//Main boss update.
	public virtual bool OnUpdate()
	{
		return false;
	}

	public virtual void OnEnter()
	{

	}

	public virtual void OnExit()
	{
	}
}
