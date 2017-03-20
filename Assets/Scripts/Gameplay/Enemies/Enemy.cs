using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the main class for the enemy. It must manage all the enemy stats. 
public class Enemy : Character {

	//Variables regarding enemy state
	public EnemyStates m_currentState;
	public EnemyStates m_Idle;
	public EnemyStates m_Following;

	//General variables
	public int m_speed = 4;

	public GameObject player;

	public override void Awake()
	{
		m_Idle = gameObject.AddComponent<EnemyIdle> ();
		m_Following = gameObject.AddComponent<EnemyFollowing> ();

		m_currentState = m_Idle;

		base.Awake ();
	}

	// Use this for initialization
	public override void Start()
	{
		base.Start ();
	}

	public override void FixedUpdate ()
	{
		if (m_currentState.OnUpdate ())
			m_currentState.OnEnter ();
	}
}
