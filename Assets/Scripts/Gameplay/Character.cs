using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public float m_maxHealth = 120.0f;
    public float m_health = 120.0f;
    public float m_moveSpeed = 4.0f;
    float m_turnSpeed;
    public float m_jumpForce = 4.0f;
    public float m_lerpSpeed = 10.0f;

    public DamageData m_damage;

    [HideInInspector] public Animator m_animator;
    [HideInInspector] public GameObjectGravity m_gravityOnCharacter;
    [HideInInspector] public Rigidbody m_rigidBody;
    [HideInInspector] public CapsuleCollider m_capsule;
    [HideInInspector] public float m_capsuleHeight;

    protected float m_groundCheckDistance;
    protected float m_defaultGroundCheckDistance = 0.3f;

    protected bool m_isJumping = false;

    protected bool m_isGrounded;

    public virtual void Awake()
    {
        if (!(m_gravityOnCharacter =  GetComponent<GameObjectGravity>()))
            m_gravityOnCharacter = gameObject.AddComponent<GameObjectGravity>();

		if(tag == "Player")
            m_animator = GetComponent<Animator>();
        else
			m_animator = transform.GetChild(0).GetComponent<Animator> ();
    }

    // Use this for initialization
    public virtual void Start ()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_capsule = GetComponent<CapsuleCollider>();
        m_capsuleHeight = m_capsule.height;

        m_rigidBody.freezeRotation = true;
        m_health = m_maxHealth;
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
    }

    // This function checks if the character is currently touching a collider below their "feet"
    public bool CheckGroundStatus()
    {
        RaycastHit hitInfo = new RaycastHit();
        Debug.DrawLine(transform.position + (transform.up * 0.1f), transform.position + (transform.up * 0.1f) + (-transform.up * m_groundCheckDistance), Color.magenta);
        if (GroundCheck(ref hitInfo))
        {
            if(this is Player)
            {
                ((Player)this).m_tagGround = hitInfo.collider.tag;
            }
            m_gravityOnCharacter.GravityOnFeet(hitInfo);
            m_isGrounded = true;
            m_isJumping = false;
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
        float fallVelocity = Vector3.Dot(m_gravityOnCharacter.m_gravity, m_rigidBody.velocity);
        m_rigidBody.velocity += m_gravityOnCharacter.m_gravity * (m_jumpForce - fallVelocity);
        m_isGrounded = false;
        m_isJumping = true;
        m_groundCheckDistance = 0.01f;
        PlayerSoundEffects m_soundEffects = GetComponent<PlayerSoundEffects>();
        if(m_soundEffects != null)
        {
            m_soundEffects.PlaySound(PlayerSoundEffects.Jump);
        }
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

    bool GroundCheck(ref RaycastHit hitInfo)
    {
        bool ret = false;

        ret = Physics.Raycast(transform.position + (transform.up * 0.1f), -transform.up, out hitInfo, m_groundCheckDistance);
        if (!ret)
            ret = Physics.SphereCast(transform.position + (transform.up * 0.1f), m_capsule.radius, -transform.up, out hitInfo, m_groundCheckDistance);

        return ret;
    }
}
