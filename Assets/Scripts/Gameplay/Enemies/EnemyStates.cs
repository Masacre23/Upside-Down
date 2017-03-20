using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates : MonoBehaviour 
{
	protected enum States
	{
		IDLE,
		FOLLOWING,
		CHANGING
	}

	protected States m_type;
	protected Enemy m_enemy;
	protected Rigidbody m_rigidBody;

	// Use this for initialization
	public virtual void Start () {
		m_enemy = GetComponent<Enemy> ();
		m_rigidBody = GetComponent<Rigidbody> ();
	}
	
	//Main enemy update.
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
