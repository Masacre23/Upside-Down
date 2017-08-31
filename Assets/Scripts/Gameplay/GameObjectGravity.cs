﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class should be added to any GameObject which gravity can be changed during the game.
//It controls the current gravity of the object, and adds it to its rigid body.
public class GameObjectGravity : MonoBehaviour
{
    [HideInInspector] public Rigidbody m_rigidBody;
    private RaycastHit m_attractor;
    public GameObject m_attractorGameObject;
    public Vector3 m_gravity { get; private set; }

    public List<GravityAttractor> m_planetsGravity;
    public List<GravityAttractor> m_objectsGravity;

    public float m_maxTimeTravelled = 0.5f;
    public float m_maxTimeOnGravityWallGravity = 1.0f;

    public bool m_ignoreGravity = false;
    float m_timeTravelled;
    float m_timeOnGravityWallGravity;
    Vector3 m_impulseForce;

    public bool m_onGravityWall = false;
    public bool m_onAir = false;

    public bool m_getStrongestGravity = true;

    //This should be the same for all gameobjects
    static float m_gravityStrength = -19.0f;

    Player m_player;
    public Vector3 test;

    void Awake()
    {
        m_planetsGravity = new List<GravityAttractor>();
        m_objectsGravity = new List<GravityAttractor>();

        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.useGravity = false;
        m_rigidBody.velocity = Vector3.zero;

        m_gravity = Vector3.zero;
    }

	// Use this for initialization
	void Start ()
    {
        m_impulseForce = Vector3.zero;
        m_attractorGameObject = null;

        m_player = GetComponent<Player>();
    }

    public void Update()
    {
        test = m_gravity;
        if (m_onGravityWall && !m_getStrongestGravity)
        {
            m_timeOnGravityWallGravity += Time.deltaTime;
            if (m_timeOnGravityWallGravity > m_maxTimeOnGravityWallGravity)
            {
                m_onGravityWall = false;
                m_timeOnGravityWallGravity = 0.0f;
            }
        }
    }

	// Called to add gravity force into the rigid body.
	public void FixedUpdate ()
    {
        if (!m_ignoreGravity)
        {
            if (!m_onGravityWall || (m_onAir && m_objectsGravity.Count > 0) )
                m_gravity = GetGravity();
                
            m_rigidBody.AddForce(m_gravityStrength * m_rigidBody.mass * m_gravity);
        }
        else
        {
            m_timeTravelled += Time.fixedDeltaTime;
            if (m_timeTravelled > m_maxTimeTravelled)
            {
                m_timeTravelled = 0.0f;
                m_ignoreGravity = false;
            }
        }
    }

    // Compute the gravity of the object
    private Vector3 GetGravity()
    {
        Vector3 gravity = Vector3.zero;

        if (m_objectsGravity.Count > 0)
        {
            if (m_getStrongestGravity)
            {
                //GameObject strongestAttractor = GetStrongestAttractor();
                foreach (GravityAttractor attractor in m_objectsGravity)
                {
                    if (attractor.m_attractor == m_attractorGameObject)
                    {
                        gravity += attractor.GetGravityWithDistance(transform.position);
                    }
                }
            }
            else
            {
                foreach (GravityAttractor attractor in m_objectsGravity)
                {
                    Vector3 newGravity = Vector3.zero;
                    float newDistance = 0.0f;
                    attractor.GetDistanceAndGravityVector(transform.position, ref newGravity, ref newDistance);

                    float intensity = 1.0f / newDistance;
                    if (intensity > 1000.0f)
                        intensity = 1000.0f;

                    gravity += newGravity * intensity;
                }
            }
        }
        else
        {
            foreach (GravityAttractor planet in m_planetsGravity)
            {
                gravity += planet.GetGravityWithDistance(transform.position);
            }
        }

        return gravity.normalized;
    }

    // Detects the strongest gravity attractor (the one the player is closest to)
    private GameObject GetStrongestAttractor()
    {
        GameObject objectAttractor = null;
        float distance = 1000.0f;

        foreach (GravityAttractor attractor in m_objectsGravity)
        {
            float attractorDistance = attractor.GetDistance(transform.position);
            if (attractorDistance < distance)
                objectAttractor = attractor.m_attractor;
        }

        return objectAttractor;
    }

    //This function sets the passed raycasthit as the current attractor for the game object
    //It's mainly to be used by characters in order update its attractor while they walk on an attractor, not on planet
    public void GravityOnFeet(RaycastHit hit)
    {
        //m_attractorGameObject = hit.transform.gameObject;
        AttractorProperties attractorProperties = hit.transform.gameObject.GetComponent<AttractorProperties>();
        if (attractorProperties)
            m_attractorGameObject = attractorProperties.m_attractor;

        if (hit.collider.tag == "GravityWall")
        {
            m_gravity = hit.normal;
            m_onGravityWall = true;
            m_timeOnGravityWallGravity = 0.0f;
        }
        else
            m_onGravityWall = false;
    }

}
