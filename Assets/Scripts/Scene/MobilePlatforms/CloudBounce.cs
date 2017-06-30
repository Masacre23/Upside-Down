using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBounce : MonoBehaviour
{
    public Transform m_platform;
    public Vector3 m_bounceDirection;
    public float m_bounceAmplitude = 0.1f;
    public float m_timeBounce = 0.5f;

    bool m_bouncing = false;
    float m_time = 0.0f;
    Vector3 m_initialPosition;

    void Start ()
    {
        m_initialPosition = m_platform.position;

        m_bounceDirection = transform.TransformDirection(m_bounceDirection);
    }
	
	void Update ()
    {
        if (m_bouncing)
        {
            m_time += Time.deltaTime;
            if (m_time > m_timeBounce)
            {
                m_bouncing = false;
                m_platform.position = m_initialPosition;
            }   
            else
            {
                float delta = Mathf.PI * m_time / m_timeBounce;
                m_platform.position = m_initialPosition + m_bounceDirection * m_bounceAmplitude * Mathf.Sin(delta);
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !m_bouncing)
        {
            m_bouncing = true;
            m_time = 0.0f;
        }
    }
}
