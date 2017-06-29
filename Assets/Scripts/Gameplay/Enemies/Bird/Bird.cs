using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : Character {
	
	//Variables regarding enemy state
	public BirdStates m_currentState;
	[HideInInspector] public BirdStates m_Idle;
	[HideInInspector] public BirdStates m_Attacking;
	//[HideInInspector] public BirdStates m_ReceivingDamage;
	//[HideInInspector] public BirdStates m_Dead;

	public DamageData m_damageData;

	//General variables
	//public int m_speed = 2;
	public GameObject player;

	public override void Awake()
	{
		m_Idle = gameObject.GetComponent<BirdIdle>();
		if (!m_Idle)
			m_Idle = gameObject.AddComponent<BirdIdle>();

		m_Attacking = gameObject.GetComponent<BirdAttacking>();
		if (!m_Attacking)
			m_Attacking = gameObject.AddComponent<BirdAttacking>();

		/*m_ReceivingDamage = gameObject.GetComponent<EnemyReceivingDamage>();
		if (!m_ReceivingDamage)
			m_ReceivingDamage = gameObject.AddComponent<EnemyReceivingDamage>();

		m_Dead = gameObject.GetComponent<EnemyDead>();
		if (!m_Dead)
			m_Dead = gameObject.AddComponent<EnemyDead>();*/

		m_currentState = m_Idle;

		m_damageData = new DamageData();

		base.Awake();
	}
		
	public override void Start()
	{
		base.Start();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		BirdStates previousState = m_currentState;
		if (m_currentState.OnUpdate(m_damageData))
		{
			previousState.OnExit();
			m_currentState.OnEnter();
		}

		m_damageData.ResetDamageData();
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.rigidbody)
		{
			ThrowableObject throwableObject = col.rigidbody.GetComponent<ThrowableObject>();
			if (throwableObject && throwableObject.m_canDamage)
			{
				throwableObject.m_canDamage = false;
				m_damageData.m_recive = true;
				m_damageData.m_damage = 50;
			}
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
			player = col.gameObject;
		}
	}

	public void DamageManager(DamageData data)
	{
		//m_isSleeping = false;
		m_animator.SetBool("Sleeping", false);
		m_animator.speed = 1;
		m_health -= data.m_damage;
		/*if (m_health <= 0)
			m_currentState = m_Dead;
		else
		{
			if (data.m_respawn)
			{
				m_currentState = m_Dead;
				gameObject.SetActive(false);
			}
			else
				m_currentState = m_ReceivingDamage;
		}*/
	}
}
