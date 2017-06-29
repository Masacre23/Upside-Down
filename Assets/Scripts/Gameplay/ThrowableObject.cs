using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    public bool m_floatLimitless = true;
    public bool m_canBePicked = true;
    public bool m_isFloating = false;
    public float m_floatingSpeed = 10.0f;
    public float m_maxTimeFloating = 10.0f;
    public float m_rotationSpeed = 100.0f;
    public GameObject m_aura;

    GameObjectGravity m_objectGravity;
    Rigidbody m_rigidBody;
    Collider m_collider;
    Transform m_floatingPoint;
    FloatingAroundPlayer m_targetPlayer;
    TrailRenderer m_trail;
   

    [HideInInspector] public bool m_canDamage = false;
    bool m_applyThrownForce = false;
    Vector3 m_thrownForce = Vector3.zero;
    float m_minVelocityDamage = 2.0f;
    float m_realTimeFloating = 10.0f;
    float m_timeFloating = 0.0f;
    Vector3 m_rotationRandomVector = Vector3.zero;

	// Use this for initialization
	void Start ()
    {
        m_objectGravity = GetComponent<GameObjectGravity>();
        m_rigidBody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        m_trail = GetComponent<TrailRenderer>();

        m_realTimeFloating = m_maxTimeFloating;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_isFloating)
        {
            //Moving the object to the floating point position
            transform.position = Vector3.Lerp(transform.position, m_floatingPoint.position, m_floatingSpeed * Time.deltaTime);

            //Random rotation of the object within itself
            transform.Rotate(m_rotationRandomVector * m_rotationSpeed * Time.deltaTime);

            // Checking if the object should fall
            if (!m_floatLimitless)
            {
                m_timeFloating += Time.deltaTime;
                if (m_timeFloating > m_maxTimeFloating)
                    StopFloating();
            } 
        }

        //Checks if the object can damage to enemies. When its velocity falls below a certain value, it stops to deal damage if collided.
        if (m_canDamage)
        {
            if (m_rigidBody.velocity.magnitude < m_minVelocityDamage)
            {
                m_canDamage = false;
            }
        }
        if(m_rigidBody.velocity.magnitude == 0 && m_trail.enabled)
        {
            m_trail.enabled = false;
        }
	}

    void FixedUpdate()
    {
        if (m_applyThrownForce)
        {
            m_rigidBody.AddForce(m_thrownForce * m_rigidBody.mass, ForceMode.Impulse);
            m_applyThrownForce = false;
        }
    }

    //This function should be called when the object is thrown
    public void ThrowObject(Vector3 throwForce)
    {
        StopFloating();

        m_thrownForce = throwForce;
        m_applyThrownForce = true;
        m_canDamage = true;

        m_objectGravity.m_ignoreGravity = true;
        if(m_trail != null)
        {
            m_trail.enabled = true;
        }
    }

    // This function should be called when an object begins to float around the character
    public void BeginFloating(Transform floatingPoint, FloatingAroundPlayer player, float timeFloating = 0.0f)
    {
        m_floatingPoint = floatingPoint;
        m_targetPlayer = player;

        m_isFloating = true;
        m_canBePicked = false;

        if (timeFloating != 0.0f)
            m_realTimeFloating = m_maxTimeFloating;
        else
            m_realTimeFloating = timeFloating;

        if (m_rigidBody)
            m_rigidBody.isKinematic = true;

        if (m_collider)
            m_collider.enabled = false;

        m_rotationRandomVector = new Vector3(Random.value, Random.value, Random.value).normalized;

        m_timeFloating = 0.0f;

        if (m_aura != null)
            m_aura.SetActive(true);
    }

    //This function should be called when an object stop floating around the character
    public void StopFloating()
    {
        m_targetPlayer.FreeSpace(m_floatingPoint);
        m_targetPlayer = null;
        m_floatingPoint = null;

        m_isFloating = false;
        m_canBePicked = true;

        if (m_rigidBody)
            m_rigidBody.isKinematic = false;

        if (m_collider)
            m_collider.enabled = true;

        m_rotationRandomVector = Vector3.zero;
        if (m_aura != null)
            m_aura.SetActive(false);
    }

    void OnCollisionEnter(Collision col)
    {
        SoundEffects sound = GetComponent<SoundEffects>();
        if (sound != null)
        {
            sound.PlaySound("HitSomething");
        }
    }


}
