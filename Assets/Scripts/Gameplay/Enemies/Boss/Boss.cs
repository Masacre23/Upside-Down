using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {
	public BossStates m_currentState;
	[HideInInspector] public BossStates m_Idle;
	[HideInInspector] public BossStates m_Attack;
	[HideInInspector] public BossStates m_Stunned;

	public Animator m_animator;
	public int m_phase;
	public float m_attackRate;
	public float m_rotationSpeed = 4.0f;
	public float m_speed = 2.0f;
	public float m_force = 10f;
	public GameObject OBJETO_TIRAR;

	public GameObject ball;
	public GameObject player;
    public float minDistanceToPlayer = 3.0f;

    public bool m_canChase = true;

	void Awake()
	{
		m_animator = gameObject.GetComponent<Animator> ();

		m_Idle = gameObject.GetComponent<BossIdle> ();
		if (!m_Idle)
			m_Idle = gameObject.AddComponent<BossIdle> ();

		m_Attack = gameObject.GetComponent<BossAttack> ();
		if (!m_Attack)
			m_Attack = gameObject.AddComponent<BossAttack> ();

		m_Stunned = gameObject.GetComponent <BossStunned> ();
		if (!m_Stunned)
			m_Stunned = gameObject.AddComponent<BossStunned> ();

		m_currentState = m_Idle;
	}

	void Start () {
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//transform.position -= new Vector3 (0, 9.8f * Time.deltaTime, 0);

		BossStates previousState = m_currentState;
		if (m_currentState.OnUpdate ()) 
		{
			previousState.OnExit ();
			m_currentState.OnEnter ();
		}
	}

	public void Stun()
	{
		m_currentState.OnExit ();
		if (m_currentState == m_Stunned)
			m_currentState = m_Idle;
		else
			m_currentState = m_Stunned;
		m_currentState.OnEnter ();
	}

	void OnCollisionEnter(Collision col)
	{
		
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
			col.gameObject.GetComponent<Player> ().m_damageData.m_recive = true;
			col.gameObject.GetComponent<Player> ().m_damageData.m_damage = 20;
		}
			
	}
}
