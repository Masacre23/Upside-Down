using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    public bool m_canBePicked = true;
    public bool m_isFloating = false;
    public bool m_isCarring = false;
    public float m_floatingSpeed = 10.0f;
    public float m_rotationSpeed = 100.0f;
    public GameObject m_aura;
    public Vector3 m_chargingPivot = Vector3.zero;
    public float m_timeToRotate = 2.0f;
    public bool m_isLazer = false;
    public bool m_stageTwo = false;

    GameObjectGravity m_objectGravity;
    Rigidbody m_rigidBody;
    Collider[] m_collider;

    Transform m_floatingPoint;

    PickedObject m_playerPicked = null;

    TrailRenderer m_trail;

    [HideInInspector] public bool m_canDamage = false;
    bool m_applyThrownForce = false;
    bool m_movingHorizontal = false;
    float m_thrownForce = 0.0f;
    float m_horizontalThrownForce = 1.0f;
    Vector3 m_vectorUp = Vector3.up;
    Vector3 m_vectorFroward = Vector3.forward;
    float m_minVelocityDamage = 2.0f;
    Vector3 m_rotationRandomVector = Vector3.zero;
    bool m_isEnemy = false;

    float m_timeRotating = 0.0f;

    public GameObject m_prefabHit1;

    // Use this for initialization
    void Start()
    {
        m_objectGravity = GetComponent<GameObjectGravity>();
        m_rigidBody = GetComponent<Rigidbody>();
        m_collider = GetComponentsInChildren<Collider>();
        m_trail = GetComponent<TrailRenderer>();

        m_rigidBody.freezeRotation = true;

        m_prefabHit1 = (GameObject)Resources.Load("Prefabs/Effects/CFX3_Hit_Misc_D (Orange)", typeof(GameObject));
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_movingHorizontal && !m_rigidBody.freezeRotation)
        {
            m_timeRotating += Time.deltaTime;
            if (m_timeRotating >= m_timeToRotate)
            {
                m_rigidBody.freezeRotation = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (m_movingHorizontal)
        {
            m_rigidBody.MovePosition(m_rigidBody.position + m_vectorFroward * Time.deltaTime * m_horizontalThrownForce);
        }
        if (m_applyThrownForce)
        {
            m_playerPicked = null;
            m_rigidBody.velocity = m_vectorUp * m_thrownForce;
            //m_rigidBody.AddForce(m_thrownForce * m_rigidBody.mass, ForceMode.Impulse);
            m_applyThrownForce = false;
            m_movingHorizontal = true;
        }
    }

    //This function should be called when the object is thrown
    public void ThrowObject(float throwForce, float horizontalThrowForce, Vector3 up, Vector3 forward)
    {
        if (m_playerPicked)
            StopCarried();

        m_rigidBody.freezeRotation = false;
        m_timeRotating = 0.0f;
        m_thrownForce = throwForce;
        m_horizontalThrownForce = horizontalThrowForce;
        m_vectorUp = up;
        m_vectorFroward = forward;
        m_applyThrownForce = true;
        m_canDamage = true;

        if (m_trail)
            m_trail.enabled = true;
    }

    public void ThrowObjectNow()
    {
        if (m_applyThrownForce)
        {
            transform.parent = null;
            m_playerPicked.FreeSpace();
            m_playerPicked = null;

            m_applyThrownForce = false;
            m_movingHorizontal = true;
        }
    }

    // This function is called when the object is picked by the player
    public void BeginCarried(Transform floatingPoint, PickedObject player)
    {
        transform.parent = floatingPoint;
        Enemy enemy = GetComponent<Enemy>();
        if (enemy) {
            transform.forward = player.transform.GetChild(0).up;
            float angle = Vector3.Angle(transform.up, -player.transform.GetChild(0).forward);
            transform.Rotate(transform.forward, angle, Space.World);
            enemy.m_animator.SetBool("Charged", true);
            enemy.enabled = false;
        }
        else if (m_isLazer)
        {
            if (m_stageTwo)
            {
                transform.forward = player.transform.GetChild(0).GetChild(0).forward;
            }
            else
            {
                transform.forward = player.transform.GetChild(0).forward;
            }
        }
        else{
            transform.up = player.transform.GetChild(0).up;
        }
        transform.position = floatingPoint.position;
        transform.Translate(m_chargingPivot);
        m_floatingPoint = floatingPoint;
        m_playerPicked = player;

        m_isCarring = true;
        m_canBePicked = false;

        m_objectGravity.m_ignoreGravity = true;
        if (m_rigidBody)
            m_rigidBody.isKinematic = true;
        for (int i = 0; i < m_collider.Length; i++)
        {
            m_collider[i].enabled = false;
        }

        if (m_aura)
            m_aura.SetActive(true);
    }

    //This function should be called when the object stop been carried by the player
    public void StopCarried()
    {
        transform.parent = null;
        m_playerPicked.FreeSpace();
        m_playerPicked = null;
        m_floatingPoint = null;

        m_isCarring = false;
        m_canBePicked = true;

        m_objectGravity.m_ignoreGravity = false;
        if (m_rigidBody)
            m_rigidBody.isKinematic = false;

        for (int i = 0; i < m_collider.Length; i++)
        {
            m_collider[i].enabled = true;
        }

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
        //int floor = LayerMask.NameToLayer("Floor");
        int water = LayerMask.NameToLayer("HarmfulTerrain");
        int player = LayerMask.NameToLayer("Player");
        int enemy = LayerMask.NameToLayer("Enemy");
        if (col.collider.gameObject.layer != player && col.collider.gameObject.layer != enemy)
        {
            StopMovingObject(col.collider.gameObject.layer == water);
        }
        
        if (col.collider.gameObject.layer == player)
        {
            m_rigidBody.freezeRotation = false;
            m_timeRotating = 0;
        }
    }

    public void StopMovingObject(bool isWater)
    {
        if (m_movingHorizontal)
        {
            Enemy enemy = GetComponent<Enemy>();
            if (enemy)
            {
                enemy.m_animator.SetBool("Charged", false);
                enemy.enabled = true;
                enemy.FallDamage(isWater);
            }
            m_rigidBody.velocity = Vector3.zero;
            m_movingHorizontal = false;
            m_canDamage = false;
            if (m_trail)
                m_trail.enabled = false;
        }
    }

}
