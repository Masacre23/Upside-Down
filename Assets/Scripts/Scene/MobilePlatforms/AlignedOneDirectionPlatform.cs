using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlatformAlignedState
{
    STOP,
    WAIT,
    MOVE,
    RETURN,
    NONE,
}

public class AlignedOneDirectionPlatform : MonoBehaviour {

    public float m_speed = 0.2f;
    public float m_distance = 2.0f;
    public float m_waitTime = 0.5f;
    public Vector3 m_direction = new Vector3(1.0f, 0.0f, 0.0f);
    public bool m_boomerang = true;
    public bool m_waitForTrigger = false;
    public TriggerPlatform m_trigger;

    private float m_distanceTraveled = 0.0f;
    private float m_timeWaited = 0.0f;
    private int m_sense = 1;
    private PlatformAlignedState m_state = PlatformAlignedState.STOP;
    private Vector3 m_speedLastUpdate;

    // Use this for initialization
    void Start() {}

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case PlatformAlignedState.STOP:
                if (!m_waitForTrigger || m_trigger.m_playerDetected)
                {
                    m_state = PlatformAlignedState.MOVE;
                }
                break;
            case PlatformAlignedState.WAIT:
                m_timeWaited += Time.deltaTime;
                if (m_timeWaited >= m_waitTime)
                {
                    m_sense = m_sense == 1 ? -1 : 1;
                    m_state = m_sense == 1 ? PlatformAlignedState.STOP : PlatformAlignedState.RETURN;
                    m_timeWaited = 0.0f;
                }
                break;
            case PlatformAlignedState.RETURN:
                m_state = PlatformAlignedState.MOVE;
                break;
            case PlatformAlignedState.MOVE:
                float distanceToMove = m_distance - m_distanceTraveled;
                if (m_speed * Time.deltaTime <= distanceToMove)
                    distanceToMove = m_speed * Time.deltaTime;
                m_distanceTraveled += distanceToMove;
                transform.Translate(m_sense * m_direction * distanceToMove);
                if (m_distanceTraveled >= m_distance)
                {
                    m_distanceTraveled = 0;
                    if (m_boomerang)
                        m_state = PlatformAlignedState.WAIT;
                    else
                        m_state = PlatformAlignedState.NONE;

                }
                PlanetGravity.AlignWithFather(gameObject);
                break;
        }
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (!m_waiting)
    //    {
    //        float distanceToMove = m_distance - m_distanceTraveled;
    //        if (m_speed * Time.deltaTime <= distanceToMove)
    //            distanceToMove = m_speed * Time.deltaTime;
    //        m_distanceTraveled += distanceToMove;
    //        transform.Translate(m_sense * m_direction * distanceToMove);
    //        if (m_distanceTraveled >= m_distance && m_boomerang)
    //        {
    //            m_distanceTraveled = 0;
    //            m_sense = m_sense == 1 ? -1 : 1;
    //            m_waiting = true;
    //        }
    //        m_speedLastUpdate = m_sense * m_direction * m_speed;
    //        PlanetGravity.AlignWithFather(gameObject);
    //    }
    //    else
    //    {
    //        m_timeWaited += Time.deltaTime;
    //        if (m_timeWaited >= m_waitTime)
    //        {
    //            m_waiting = false;
    //            m_timeWaited = 0.0f;
    //        }
    //        m_speedLastUpdate = Vector3.zero;
    //    }
    //}

}
