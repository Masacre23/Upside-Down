using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum PlatformRotatedState
{
    STOP,
    WAIT,
    ROTATE,
}

public class RotatedMobilePlatform : MonoBehaviour
{
    public bool m_startWithPlayer = false;
    public float m_speed = 0.2f;
    public float m_angle = 10.0f;
    public float m_waitTime = 0.5f;
    public Vector3 m_axis = new Vector3(1.0f, 0.0f, 0.0f);
    public bool m_boomerang = true;

    private float m_angleRotated = 0.0f;
    private float m_timeWaited = 0.0f;
    private int m_sense = 1;
    private bool m_playerDetected = false;
    private PlatformRotatedState m_state = PlatformRotatedState.STOP;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case PlatformRotatedState.STOP:
                if (m_playerDetected || !m_startWithPlayer)
                    m_state = PlatformRotatedState.ROTATE;
                break;
            case PlatformRotatedState.WAIT:
                m_timeWaited += Time.deltaTime;
                if (m_timeWaited >= m_waitTime)
                {
                    m_state = PlatformRotatedState.ROTATE;
                    m_timeWaited = 0.0f;
                }
                break;
            case PlatformRotatedState.ROTATE:
                m_playerDetected = false;
                float angleToRotate = m_angle - m_angleRotated;
                if (m_speed * Time.deltaTime <= angleToRotate)
                    angleToRotate = m_speed * Time.deltaTime;
                m_angleRotated += angleToRotate;
                transform.Rotate(m_sense * m_axis * angleToRotate);
                if (m_angleRotated >= m_angle && m_boomerang)
                {
                    m_angleRotated = 0;
                    if (m_sense > 0)
                        m_state = PlatformRotatedState.WAIT;
                    else
                        m_state = PlatformRotatedState.STOP;
                    m_sense = m_sense == 1 ? -1 : 1;
                }
                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
            m_playerDetected = true;
    }

}
