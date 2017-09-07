using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    public bool m_canBePicked = true;
    public bool m_isFloating = false;
    public float m_floatingSpeed = 10.0f;
    public float m_rotationSpeed = 100.0f;
    public GameObject m_aura;

    GameObjectGravity m_objectGravity;
    Rigidbody m_rigidBody;
    Collider m_collider;
    Transform m_floatingPoint;

    FloatingAroundPlayer m_targetPlayer = null;
    PickedObject m_playerPicked = null;

    TrailRenderer m_trail;
   

    [HideInInspector] public bool m_canDamage = false;
    bool m_applyThrownForce = false;
    bool m_movingHorizontal = false;
    float m_thrownForce = 0.0f;
    Vector3 m_vectorUp = Vector3.up;
    Vector3 m_vectorFroward = Vector3.forward;
    float m_minVelocityDamage = 2.0f;
    Vector3 m_rotationRandomVector = Vector3.zero;

	public GameObject m_prefabHit1;

	// Use this for initialization
	void Start ()
    {
        m_objectGravity = GetComponent<GameObjectGravity>();
        m_rigidBody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        m_trail = GetComponent<TrailRenderer>();

		m_prefabHit1 = (GameObject)Resources.Load ("Prefabs/Effects/CFX3_Hit_Misc_D (Orange)", typeof(GameObject));
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_isFloating)
        {
            //Moving the object to the floating point position
            //transform.position = Vector3.Lerp(transform.position, m_floatingPoint.position, m_floatingSpeed * Time.deltaTime);

            //Random rotation of the object within itself
            //transform.Rotate(m_rotationRandomVector * m_rotationSpeed * Time.deltaTime);
        }

        //Checks if the object can damage to enemies. When its velocity falls below a certain value, it stops to deal damage if collided.
        if (m_canDamage)
        {
            if (m_rigidBody.velocity.magnitude < m_minVelocityDamage)
            {
                m_canDamage = false;
                if (m_trail)
                    m_trail.enabled = false;
            }
        }

	}

    void FixedUpdate()
    {
        if (m_applyThrownForce)
        {
            m_rigidBody.velocity = m_vectorUp * m_thrownForce;
            //m_rigidBody.AddForce(m_thrownForce * m_rigidBody.mass, ForceMode.Impulse);
            m_applyThrownForce = false;
            m_movingHorizontal = true;
        }
        if (m_movingHorizontal)
        {
            m_rigidBody.MovePosition(m_rigidBody.position + m_vectorFroward * Time.deltaTime);
        }
    }

    //This function should be called when the object is thrown
    public void ThrowObject(float throwForce, Vector3 up, Vector3 forward)
    {
        if (m_playerPicked)
            StopCarried();
        else if (m_targetPlayer)
            StopFloating();

        m_thrownForce = throwForce;
        m_vectorUp = up;
        m_vectorFroward = forward;
        m_applyThrownForce = true;
        m_canDamage = true;

        m_objectGravity.m_ignoreGravity = true;
        if (m_trail)
            m_trail.enabled = true;
    }

    // This function should be called when an object begins to float around the character
    public void BeginFloating(Transform floatingPoint, FloatingAroundPlayer player, float timeFloating = 0.0f)
    {
        m_floatingPoint = floatingPoint;
        m_targetPlayer = player;

        m_isFloating = true;
        m_canBePicked = false;

        if (m_rigidBody)
            m_rigidBody.isKinematic = true;

        if (m_collider)
            m_collider.enabled = false;

        m_rotationRandomVector = new Vector3(Random.value, Random.value, Random.value).normalized;

        if (m_aura != null)
            m_aura.SetActive(true);
    }

    // This function is called when the object is picked by the player
    public void BeginCarried(Transform floatingPoint, PickedObject player)
    {
        transform.position = floatingPoint.position;
        transform.parent = floatingPoint;
        m_floatingPoint = floatingPoint;
        m_playerPicked = player;

        m_isFloating = true;
        m_canBePicked = false;

        if (m_rigidBody)
            m_rigidBody.isKinematic = true;

        if (m_collider)
            m_collider.enabled = false;

        if (m_aura)
            m_aura.SetActive(true);
    }

    //This function should be called when an object stop floating around the character
    public void StopFloating()
    {
        transform.parent = null;
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

    //This function should be called when the object stop been carried by the player
    public void StopCarried()
    {
        transform.parent = null;
        m_playerPicked.FreeSpace();
        m_playerPicked = null;
        m_floatingPoint = null;

        m_isFloating = false;
        m_canBePicked = true;

        if (m_rigidBody)
            m_rigidBody.isKinematic = false;

        if (m_collider)
            m_collider.enabled = true;

        if (m_aura != null)
            m_aura.SetActive(false);
    }

    void OnCollisionEnter(Collision col)
    {
        SoundEffects sound = GetComponent<SoundEffects>();
        if (sound != null)
        {
            sound.PlaySound("HitSomething");
			if(transform.tag != "EnemySnail")
				EffectsManager.Instance.GetEffect(m_prefabHit1, col.transform.position, transform.up, null);
        }
        if(col.collider.tag == "Ice")
        {
            m_movingHorizontal = false;
        }
    }


}
