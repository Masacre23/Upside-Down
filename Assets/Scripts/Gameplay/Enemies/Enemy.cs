using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the main class for the enemy. It must manage all the enemy stats. 
public class Enemy : Character {

	//Variables regarding enemy state
	public EnemyStates m_currentState;
	public EnemyStates m_Idle;
	public EnemyStates m_Following;
	public EnemyStates m_Changing;

	//General variables
	public int m_speed = 2;
	public GameObject currentWall;

	public GameObject player;

	public override void Awake()
	{
		m_Idle = gameObject.AddComponent<EnemyIdle> ();
		m_Following = gameObject.AddComponent<EnemyFollowing> ();
		m_Changing = gameObject.AddComponent<EnemyChanging> ();

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

		UpdateUp ();
	}

	void OnCollisionEnter(Collision col)
	{
		/*if (col.gameObject.tag == "EnemyWall") 
		{
			m_currentState.OnExit ();
			m_currentState = m_Changing;
			m_currentState.OnEnter ();
		}*/
		if (m_currentState == m_Changing) 
		{
			m_currentState.OnExit ();
			m_currentState.OnEnter ();
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player" && m_currentState != m_Changing) {
			m_currentState.OnExit ();
			m_currentState = m_Following;
			player = col.gameObject;
			m_currentState.OnEnter ();
		}

		if (col.tag == "EnemyWall") 
		{
			//m_Following.wallToChange = col.gameObject;
			if (m_currentState == m_Following) 
			{
				m_currentState.OnExit ();
				m_currentState = m_Changing;
				m_currentState.OnEnter ();
			}
		}
	}
}
