using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the main class for the enemy. It must manage all the enemy stats. 
public class Enemy : Character {

    //Variables regarding enemy state
    public EnemyStates m_currentState;
    [HideInInspector] public EnemyStates m_Idle;
    [HideInInspector] public EnemyStates m_Following;
    [HideInInspector] public EnemyStates m_Stunned;
    [HideInInspector] public EnemyStates m_ReceivingDamage;
    [HideInInspector] public EnemyStates m_Dead;
    // [HideInInspector] public EnemyStates m_Attacking;

    public DamageData m_damageData;
	public Renderer m_renderer;
	public Renderer m_rendererShell;
	public Material m_transparentMat;

    //General variables
	public Collider m_headCol;
	public Collider m_activeCollider;
    public int m_speed = 2;
    public bool m_isFloating = false;

    public GameObject player;
    public bool m_isSleeping = false;
	public GameObject m_enemyArea;

    [HideInInspector]public bool m_wasStunned = false;
    private bool m_playerDetected = true;
    public EnemyAttacCollider m_enemyCollider;
    [HideInInspector]public EnemySoundEffects m_sound;

    public enum Types
    {
        SNAIL,
        FLYING
    }

    public Types m_type;

    public override void Awake()
    {
        m_Idle = gameObject.GetComponent<EnemyIdle>();
        if (!m_Idle)
            m_Idle = gameObject.AddComponent<EnemyIdle>();

        m_Following = gameObject.GetComponent<EnemyFollowing>();
        if (!m_Following)
            m_Following = gameObject.AddComponent<EnemyFollowing>();

        /* m_Changing = gameObject.GetComponent<EnemyChanging>();
         if (!m_Changing)
             m_Changing = gameObject.AddComponent<EnemyChanging>();*/

        m_ReceivingDamage = gameObject.GetComponent<EnemyReceivingDamage>();
        if (!m_ReceivingDamage)
            m_ReceivingDamage = gameObject.AddComponent<EnemyReceivingDamage>();

        m_Dead = gameObject.GetComponent<EnemyDead>();
        if (!m_Dead)
            m_Dead = gameObject.AddComponent<EnemyDead>();

        m_Stunned = gameObject.GetComponent<EnemyStunned>();
        if (!m_Stunned)
            m_Stunned = gameObject.AddComponent<EnemyStunned>();

        /*if (!m_Attacking)
             m_Attacking = gameObject.AddComponent<FlyingEnemyAttacking>();*/

        m_currentState = m_Idle;

        m_damageData = new DamageData();

        m_enemyCollider = GetComponentInChildren<EnemyAttacCollider>();

        m_sound = GetComponent<EnemySoundEffects>();

        base.Awake();
    }

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        EnemyStates previousState = m_currentState;
        if (m_currentState.OnUpdate(m_damageData, m_wasStunned))
        {
            previousState.OnExit();
            m_currentState.OnEnter();
        }

		/*if(m_activeCollider == m_headCol)
		{
			if (!m_animator.GetCurrentAnimatorStateInfo (0).IsName ("Attack")) 
			{
				m_animator.SetBool ("Stunned", true);
				this.gameObject.transform.GetChild (3).gameObject.SetActive (true);
				this.gameObject.transform.GetChild (3).gameObject.GetComponent<ParticleSystem> ().Play ();
			}
			m_activeCollider = null;
		}*/

        UpdateUp();

        m_damageData.ResetDamageData();
    }

    void OnCollisionEnter(Collision col)
    {
        ThrowableObject throwableObject = col.gameObject.GetComponent<ThrowableObject>();
        if (throwableObject && throwableObject.m_canDamage)
		{
            throwableObject.m_canDamage = false;
            m_damageData.m_recive = true;
            m_damageData.m_damage = 50;
            throwableObject.StopMovingObject(false);
			CalculateDirection (col.gameObject, this.gameObject);
        }
        
  //      if (col.gameObject.tag == "Player") 
		//{
		//	if (m_animator.GetCurrentAnimatorStateInfo (0).IsName ("Attack")) 
		//	{
		//		col.gameObject.GetComponent<Player> ().m_damageData.m_recive = true;
		//		col.gameObject.GetComponent<Player> ().m_damageData.m_damage = 20;

		//		Vector3 diff = transform.position - col.transform.position;
		//		float distance = diff.magnitude;
		//		Vector3 dir = diff / distance;

		//		RaycastHit hit;
		//		if (Physics.Raycast (transform.position, dir, out hit, 1f)) {
		//			EffectsManager.Instance.GetEffect (m_prefabHit1, col.transform.position + transform.up / 2 + col.transform.forward / 2, transform.up, null);
		//		}
		//	}
		//}

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
		if (col.tag == "Player")
        {
            player = col.gameObject;
            m_isSleeping = false;
            m_animator.SetBool("Sleeping", false);
            m_animator.speed = 1;
        }
    }

    public void CanAttack(bool attack)
    {
        //m_enemyCollider.CanAttack(attack);
    }

    public void DamageManager(DamageData data)
    {
        m_isSleeping = false;
        m_animator.SetBool("Sleeping", false);
        m_animator.speed = 1;
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

    public void FallDamage(bool is_water)
    {
        m_damageData.m_recive = true;
        m_damageData.m_damage = 25;
        if (is_water)
        {
            m_damageData.m_damage = (int)(m_health + 1);
        }
    }

    public void Stun()
    {
        m_wasStunned = true;
    }

    public enum HitDirection { None, Top, Bottom, Forward, Back, Left, Right }
	public HitDirection hitDirection = HitDirection.None;
	public Vector3 MyNormal;
	void CalculateDirection( GameObject Object, GameObject ObjectHit ){

		//hitDirection = HitDirection.None;
		/*RaycastHit MyRayHit;
		Vector3 direction = ( Object.transform.position - ObjectHit.transform.position ).normalized;
		Ray MyRay = new Ray( ObjectHit.transform.position, direction );

		if ( Physics.Raycast( MyRay, out MyRayHit ) ){

			if ( MyRayHit.collider != null ){

				MyNormal = MyRayHit.normal;
				MyNormal = MyRayHit.transform.TransformDirection( MyNormal );

				if( MyNormal == MyRayHit.transform.up ){ hitDirection = HitDirection.Top; }
				if( MyNormal == -MyRayHit.transform.up ){ hitDirection = HitDirection.Bottom; }
				if( MyNormal == MyRayHit.transform.forward ){ hitDirection = HitDirection.Forward; }
				if( MyNormal == -MyRayHit.transform.forward ){ hitDirection = HitDirection.Back; }
				if( MyNormal == MyRayHit.transform.right ){ hitDirection = HitDirection.Right; }
				if( MyNormal == -MyRayHit.transform.right ){ hitDirection = HitDirection.Left; }
			}    
		}*/
		RaycastHit MyRayHit;
		Vector3 direction = ( Object.transform.position - ObjectHit.transform.position ).normalized;
		Ray MyRay = new Ray( ObjectHit.transform.position, direction );

		if (Physics.Raycast (MyRay, out MyRayHit)) {
			Vector3 localPoint = MyRayHit.transform.InverseTransformPoint (MyRayHit.point);
			Vector3 localDir = localPoint.normalized;

			float upDot = Vector3.Dot(localDir, Vector3.up);
			float fwdDot = Vector3.Dot(localDir, Vector3.forward);
			float rightDot = Vector3.Dot(localDir, Vector3.right);

			float upPower = Mathf.Abs(upDot);
			float fwdPower = Mathf.Abs(fwdDot);
			float rightPower = Mathf.Abs(rightDot);

			if (upPower > fwdPower) 
			{
				if (upPower > rightPower) 
				{
					if (upDot > 0)
						hitDirection = HitDirection.Top;
					else
						hitDirection = HitDirection.Bottom;
				} else 
				{
					if (rightDot > 0)
						hitDirection = HitDirection.Right;
					else
						hitDirection = HitDirection.Left;
				}
			} else if (fwdPower > rightPower) 
			{
				if (fwdDot > 0)
					hitDirection = HitDirection.Forward;
				else
					hitDirection = HitDirection.Back;
			} else 
			{
				if (rightDot > 0)
					hitDirection = HitDirection.Right;
				else
					hitDirection = HitDirection.Left;
			}
		}

	}

   
}
