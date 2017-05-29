using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the main class for the enemy. It must manage all the enemy stats. 
public class Enemy : Character {

    //Variables regarding enemy state
    public EnemyStates m_currentState;
    [HideInInspector] public EnemyStates m_Idle;
    [HideInInspector] public EnemyStates m_Following;
    [HideInInspector] public EnemyStates m_Changing;
    [HideInInspector] public EnemyStates m_ReceivingDamage;
    [HideInInspector] public EnemyStates m_Dead;

    public DamageData m_damageData;

    //General variables
    public int m_speed = 2;
	public BoxCollider m_patrollingArea;
    //public GameObject currentWall;
    public bool m_isFloating = false;

	//public Animator m_animator;
	public GameObject player;

	public override void Awake()
	{
        m_Idle = gameObject.GetComponent<EnemyIdle>();
        if (!m_Idle)
            m_Idle = gameObject.AddComponent<EnemyIdle>();

        m_Following = gameObject.GetComponent<EnemyFollowing>();
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
            m_Dead = gameObject.AddComponent<EnemyDead>();

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

        int harmfulTerrain = LayerMask.NameToLayer("HarmfulTerrain");
        if (col.collider.gameObject.layer == harmfulTerrain)
        {
            m_damageData.m_recive = true;
            m_damageData.m_damage = 20;
            m_damageData.m_respawn = true;
        }
    }

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player" && m_currentState != m_Changing)
        {
			m_currentState.OnExit();
			m_currentState = m_Following;
			player = col.gameObject;
			m_currentState.OnEnter();
		}

		if (col.tag == "EnemyWall") 
		{
			if (m_currentState == m_Following) 
			{
				m_currentState.OnExit ();
				m_currentState = m_Changing;
				m_currentState.OnEnter();
			}
		}
	}

    public void DamageManager(DamageData data)
    {
        m_health -= data.m_damage;
        if (m_health <= 0)
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
        }
    }
}
