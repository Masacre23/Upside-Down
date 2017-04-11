using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlatformDetroyableState
{
    STOP,
    WAIT, 
    DESTROY, 
    NEW,
}

public class DestroyablePlatform : MonoBehaviour {

    public float m_speed = 0.2f;
    public float m_distance = 2.0f;
    public float m_waitTime = 0.5f;
    public Vector3 m_direction = new Vector3(1.0f, 0.0f, 0.0f);

    private Vector3 m_position;
    private float m_distanceTraveled = 0.0f;
    private float m_timeWaited = 0.0f;
    private bool m_playerDetected;
    private PlatformDetroyableState m_state = PlatformDetroyableState.STOP;

    // Use this for initialization
    void Start()
    {
        m_position = transform.position;
        m_playerDetected = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_state)
        {
            case PlatformDetroyableState.STOP:
                if (m_playerDetected)
                    m_state = PlatformDetroyableState.WAIT;
                break;
            case PlatformDetroyableState.WAIT:
                m_playerDetected = false;
                m_timeWaited += Time.deltaTime;
                if (m_timeWaited >= m_waitTime)
                {
                    m_state = PlatformDetroyableState.DESTROY;
                    m_timeWaited = 0.0f;
                }
                break;
            case PlatformDetroyableState.DESTROY:
                float distanceToMove = m_distance - m_distanceTraveled;
                if (m_speed * Time.deltaTime <= distanceToMove)
                    distanceToMove = m_speed * Time.deltaTime;
                m_distanceTraveled += distanceToMove;
                transform.Translate( m_direction * distanceToMove);
                if (m_distanceTraveled >= m_distance)
                {
                    m_distanceTraveled = 0;
                    m_state = PlatformDetroyableState.NEW;
                }
                break;
            case PlatformDetroyableState.NEW:
                transform.position = m_position;
                m_state = PlatformDetroyableState.STOP;
                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" )
            m_playerDetected = true;
    }
}
