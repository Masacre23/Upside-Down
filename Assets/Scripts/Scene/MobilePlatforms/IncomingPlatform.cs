using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum IncomingPlatformState
{
    NOT_EXIST,
    INCOMING,
    READY,
}

public class IncomingPlatform : MonoBehaviour
{

    public float m_growthRate = 0.2f;
    public TriggerPlatform m_trigger;

    private IncomingPlatformState m_state = IncomingPlatformState.NOT_EXIST;
    private Vector3 m_scale;

    private TrailRenderer m_trail;

    // Use this for initialization
    void Start ()
    {
        m_scale = new Vector3(0.0f, 0.0f, 0.0f);
        transform.localScale = m_scale;

        m_trail = GetComponent<TrailRenderer>();
        if (m_trail)
            m_trail.enabled = false;

        if (!m_trigger)
            m_state = IncomingPlatformState.INCOMING;
    }
	
	// Update is called once per frame
	void Update () {
        switch (m_state)
        {
            case IncomingPlatformState.NOT_EXIST:
                if(m_trigger.m_playerDetected)
                {
                    m_state = IncomingPlatformState.INCOMING;
                }
                break;
            case IncomingPlatformState.INCOMING:
                if (m_trail)
                    m_trail.enabled = true;
                m_scale += (new Vector3(1.0f, 1.0f, 1.0f) * m_growthRate * Time.deltaTime);
                transform.localScale = m_scale;
                if(transform.localScale.x > 1.0f)
                {
                    m_scale = new Vector3(1.0f, 1.0f, 1.0f);
                    transform.localScale = m_scale;
                    m_state = IncomingPlatformState.READY;
                }
                break;
        }
	}
}
