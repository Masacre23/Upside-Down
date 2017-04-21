using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public bool m_isThrowable = false;
    public float m_maxHealth = 120.0f;
    public float m_health = 120.0f;
    public float m_moveSpeed = 4.0f;
    float m_turnSpeed;
    public float m_jumpForce = 4.0f;
    public float m_lerpSpeed = 10.0f;
    public Transform m_checkPoint;

    public bool m_damageRecive = false;
    public int m_damagePower = 0;
    public bool m_respawn = false;
    public bool m_alive = true;

	public Animator m_animator;
    public GameObjectGravity m_gravityOnCharacter;
    protected Rigidbody m_rigidBody;
    CapsuleCollider m_capsule;
    public float m_capsuleHeight;

    //Variables regarding damage state
    public DamageStates m_damageState;
    public DamageStates m_recive;
    public DamageStates m_animation;
    public DamageStates m_damageRespawn;
    public DamageStates m_notRecive;
    public DamageStates m_dead;

    protected float m_groundCheckDistance;
    protected float m_defaultGroundCheckDistance = 0.3f;

    protected bool m_isGrounded;

    public virtual void Awake()
    {
        if (!(m_gravityOnCharacter =  GetComponent<GameObjectGravity>()))
        {
            m_gravityOnCharacter = gameObject.AddComponent<GameObjectGravity>();
            m_gravityOnCharacter.m_canBeThrowed = m_isThrowable;
        }   

		if(tag == "Player")
			m_animator = transform.GetChild(0).GetChild(1).GetComponent<Animator> ();
		else
			m_animator = transform.GetChild(0).GetComponent<Animator> ();

        m_recive = gameObject.AddComponent<DamageRecive>();
        m_animation = gameObject.AddComponent<DamageAnimation>();
        m_damageRespawn = gameObject.AddComponent<DamageRespawn>();
        m_notRecive = gameObject.AddComponent<DamageNotRecive>();
        m_dead = gameObject.AddComponent<DamageDead>();

        m_damageState = m_recive;
    }

    // Use this for initialization
    public virtual void Start ()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_capsule = GetComponent<CapsuleCollider>();
        m_capsuleHeight = m_capsule.height;

        m_rigidBody.freezeRotation = true;
	}

    public virtual void Restart()
    {
        m_rigidBody.velocity = Vector3.zero;
        m_gravityOnCharacter.m_planetGravity = true;
        m_gravityOnCharacter.m_changingToAttractor = false;
    }

    public virtual void Update()
    {
    }

    // We use FixedUpdate since we will be dealing with forces
    // This method should control character's movements
    public virtual void FixedUpdate()
    {
        DamageStates previousState = m_damageState;
        if (m_damageState.OnUpdate(m_damageRecive, m_damagePower, m_damageRespawn, m_alive))
        {
            previousState.OnExit();
            m_damageState.OnEnter();
        }
        m_damagePower = 0;
        m_damageRecive = false;
	}

    // This function checks if the character is currently touching a collider below their "feet"
    public bool CheckGroundStatus()
    {
        RaycastHit hitInfo;
        Debug.DrawLine(transform.position + (transform.up * 0.1f), transform.position + (transform.up * 0.1f) + (-transform.up * m_groundCheckDistance), Color.magenta);
        if (Physics.Raycast(transform.position + (transform.up * 0.1f), -transform.up, out hitInfo, m_groundCheckDistance))
        {
            m_gravityOnCharacter.GravityOnFeet(hitInfo);
            m_isGrounded = true;
        }
        else
        {
            m_isGrounded = false;
        }

        return m_isGrounded;
    }

    //This function rotates the character so its Vector.up aligns with the direction of the attractor's gravity
    public void UpdateUp()
    {
        Quaternion targetRot = Quaternion.FromToRotation(transform.up, m_gravityOnCharacter.m_gravity) * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, m_lerpSpeed * Time.fixedDeltaTime);
    }

    //This function deals with the jump of the character
    //It mainly adds a velocity to the rigidbody in the direction of the gravity.
    public void Jump()
    {
        m_rigidBody.velocity += m_gravityOnCharacter.m_gravity * m_jumpForce;
        m_isGrounded = false;
        m_groundCheckDistance = 0.1f;
    }

    //This function should be called while character is on air.
    //It controls the detection of the floor. If the character is going up, the detection is small in order to avoid being unable to jump.
    public void OnAir()
    {
        //m_groundCheckDistance = Vector3.Dot(m_rigidBody.velocity, transform.up) < 0 ? m_defaultGroundCheckDistance : 0.01f;

        if (Vector3.Dot(m_rigidBody.velocity, transform.up) < 0)
        {
            m_groundCheckDistance = m_defaultGroundCheckDistance;
            if (!m_gravityOnCharacter.m_changingToAttractor)
            {
                m_gravityOnCharacter.m_planetGravity = true;
            }
        }
        else
            m_groundCheckDistance = 0.01f;
    }
}
