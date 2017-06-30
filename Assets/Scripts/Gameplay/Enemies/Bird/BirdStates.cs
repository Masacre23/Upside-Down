using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdStates : MonoBehaviour 
{
	protected enum States
	{
		IDLE,
		RECEIVING,
		ATTACKING,
		DEAD
	}

	protected States m_type;
	protected Bird m_bird;
	protected Rigidbody m_rigidBody;

	// Use this for initialization
	public virtual void Start ()
	{
		m_bird = GetComponent<Bird> ();
		m_rigidBody = GetComponent<Rigidbody> ();
	}

	//Main enemy update.
	public virtual bool OnUpdate(DamageData data)
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