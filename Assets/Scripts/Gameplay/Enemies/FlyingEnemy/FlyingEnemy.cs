using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Character {

	//Variables regarding enemy state
	public EnemyStates m_currentState;
	[HideInInspector] public EnemyStates m_Idle;
    [HideInInspector] public EnemyStates m_Attacking;
    //[HideInInspector] public EnemyStates m_Following;
    //[HideInInspector] public EnemyStates m_Changing;
    //[HideInInspector] public EnemyStates m_ReceivingDamage;
    //[HideInInspector] public EnemyStates m_Dead;

    public DamageData m_damageData;

	//General variables
	public int m_speed = 2;
	public BoxCollider m_patrollingArea;
	public GameObject player;

	public override void Awake()
	{
		m_Idle = gameObject.GetComponent<FlyingEnemyIdle>();
		if (!m_Idle)
			m_Idle = gameObject.AddComponent<FlyingEnemyIdle>();

        if (!m_Attacking)
            m_Attacking = gameObject.AddComponent<FlyingEnemyAttacking>();

        /*m_Following = gameObject.GetComponent<EnemyFollowing>();
		if (!m_Following)
			m_Following = gameObject.AddComponent<EnemyFollowing>();

		m_Changing = gameObject.GetComponent<EnemyChanging>();
		if (!m_Changing)
			m_Changing = gameObject.AddComponent<EnemyChanging>();

		m_ReceivingDamage = gameObject.GetComponent<EnemyReceivingDamage>();
		if (!m_ReceivingDamage)
			m_ReceivingDamage = gameObject.AddComponent<EnemyReceivingDamage>();

		m_Dead = gameObject.GetComponent<EnemyDead>();
		if (!m_Dead)
			m_Dead = gameObject.AddComponent<EnemyDead>();*/

        m_currentState = m_Idle;

		m_damageData = new DamageData();

		base.Awake ();
	}

	// Use this for initialization
	public override void Start()
	{
		base.Start ();
	}
	
	public override void FixedUpdate ()
	{
		base.FixedUpdate();

		EnemyStates previousState = m_currentState;
		if (m_currentState.OnUpdate(m_damageData))
		{
			previousState.OnExit();
			m_currentState.OnEnter();
		}

		UpdateUp();

		m_damageData.ResetDamageData();
	}
}
