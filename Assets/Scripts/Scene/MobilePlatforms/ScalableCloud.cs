using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ScalableCloudState
{
    GROWING,
    LIVING,
    DISAPPEARING,
    STOP,
}

public class ScalableCloud : MonoBehaviour {

    public Animator m_animator;
    public float m_delayTimeStart = 0.0f;
    public float m_lifeTime = 1.0f;
    public float m_deadTime = 1.0f;
    public bool m_startWithTrigger = true;

    private float m_time = 0.0f;
    private float m_timeWaited = 0.0f;
    private bool m_start = true;
    private ScalableCloudState m_lastState = ScalableCloudState.DISAPPEARING;
    private ScalableCloudState m_state = ScalableCloudState.STOP;
    private bool m_running = true;
    private Collider m_collider;

    // Use this for initialization
    void Start()
    {
        m_collider = GetComponent<Collider>();
        m_animator.SetInteger("State", -1);
        if (m_delayTimeStart > 0.0f)
        {
            m_running = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_running)
        {
            switch (m_state)
            {
                case ScalableCloudState.STOP:
                    if (!m_startWithTrigger || m_start)
                    {
                        m_state = ScalableCloudState.LIVING;
                        m_start = false;
                    }
                    break;
                case ScalableCloudState.GROWING:
                    if (m_collider.enabled)
                    {
                        m_lastState = m_state;
                        m_state = ScalableCloudState.LIVING;
                    }
                    break;
                case ScalableCloudState.LIVING:
                    m_time += Time.deltaTime;
                    if (m_lastState == ScalableCloudState.GROWING && m_time > m_lifeTime)
                    {
                        m_animator.SetInteger("State", -1);
                        m_state = ScalableCloudState.DISAPPEARING;
                        m_lastState = ScalableCloudState.LIVING;
                        m_time = 0;
                    }
                    else if (m_lastState == ScalableCloudState.DISAPPEARING && m_time > m_deadTime)
                    {
                        m_animator.SetInteger("State", 1);
                        m_state = ScalableCloudState.GROWING;
                        m_lastState = ScalableCloudState.LIVING;
                        m_time = 0;
                    }
                    break;
                case ScalableCloudState.DISAPPEARING:
                    if (!m_collider.enabled)
                    {
                        m_lastState = m_state;
                        m_state = ScalableCloudState.STOP;
                    }
                    break;
            }
        }
        else
        {
            m_time += Time.deltaTime;
            if (m_time > m_delayTimeStart)
            {
                m_running = true;
                m_time = 0.0f;
            }
        }

    }

    public void CloudEnableDisable()
    {
        if (m_collider)
            m_collider.enabled = !m_collider.enabled;
    }

    public void StartGrowing()
    {
        m_start = true;
    }
}
