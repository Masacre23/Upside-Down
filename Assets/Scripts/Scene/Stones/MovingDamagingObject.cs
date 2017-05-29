using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDamagingObject : MonoBehaviour
{
    public int m_impactDamage = 20;
    public float m_forceMultiplier = 1.0f;
    public Vector3 m_directionMovement;

    Vector3 m_lastFramePosition;

	// Use this for initialization
	void Start ()
    {
        m_lastFramePosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_directionMovement = (transform.position - m_lastFramePosition).normalized;

        m_lastFramePosition = transform.position;
	}
}
