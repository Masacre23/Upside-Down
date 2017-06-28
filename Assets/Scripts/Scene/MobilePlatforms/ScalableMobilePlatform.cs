using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlatformScalableState
{
    GROWING,
    LIVING,
    DISAPPEARING,
    STOP,
}

public class ScalableMobilePlatform : MonoBehaviour {

    public float m_speed = 0.2f;
    public float m_minSize = 0.01f;
    public float m_maxSize = 1.0f;
    public float m_growthRate = 0.2f;
    public float m_lifeTime = 1.0f;
    public float m_deadTime = 1.0f;
    public Vector3 m_direction = new Vector3(1.0f, 0.0f, 0.0f);
    public bool m_startWithTrigger = true;

    private Vector3 m_scale;
    private Vector3 m_position;
    private float m_time = 0.0f;
    private float m_timeWaited = 0.0f;
    private bool m_start = true;
    private PlatformScalableState m_lastState = PlatformScalableState.DISAPPEARING;
    private PlatformScalableState m_state = PlatformScalableState.STOP;

    // Use this for initialization
    void Start()
    {
        m_position = transform.position;
        m_scale = new Vector3(m_minSize, m_minSize, m_minSize);
        transform.localScale = m_scale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(m_direction * m_speed * Time.deltaTime);
        switch (m_state)
        {
            case PlatformScalableState.STOP:
                if(!m_startWithTrigger || m_start)
                {
                    m_state = PlatformScalableState.LIVING;
                    m_start = false;
                }
                break;
            case PlatformScalableState.GROWING:
                m_scale += (new Vector3(1.0f, 1.0f, 1.0f) * m_growthRate * Time.deltaTime);
                transform.localScale = m_scale;
                if (m_scale.x > m_maxSize)
                {
                    transform.localScale = new Vector3(m_maxSize, m_maxSize, m_maxSize);
                    m_lastState = m_state;
                    m_state = PlatformScalableState.LIVING;
                }
                break;
            case PlatformScalableState.LIVING:
                m_time += Time.deltaTime;
                if(m_lastState == PlatformScalableState.GROWING && m_time > m_lifeTime)
                {
                    m_state = PlatformScalableState.DISAPPEARING;
                    m_lastState = PlatformScalableState.LIVING;
                    m_time = 0;
                }
                else if(m_lastState == PlatformScalableState.DISAPPEARING && m_time > m_deadTime)
                {
                    m_state = PlatformScalableState.GROWING;
                    m_lastState = PlatformScalableState.LIVING;
                    m_time = 0;
                }
                break;
            case PlatformScalableState.DISAPPEARING:
                m_scale -= (new Vector3(1.0f, 1.0f, 1.0f) * m_growthRate * Time.deltaTime);
                transform.localScale = m_scale;
                if (m_scale.x < m_minSize)
                {
                    transform.localScale = new Vector3(m_minSize, m_minSize, m_minSize);
                    transform.position = m_position;
                    m_lastState = m_state;
                    m_state = PlatformScalableState.STOP;
                }
                break;
        }
    }

    public void StartGrowing()
    {
        m_start = true;
    }
}
