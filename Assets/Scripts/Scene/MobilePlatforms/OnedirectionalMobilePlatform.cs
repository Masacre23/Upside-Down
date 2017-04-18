using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlatformOneDirectionState
{
    STOP,
    WAIT,
    MOVE,
    RETURN,
}

public class OnedirectionalMobilePlatform : MonoBehaviour {

    public float m_speed = 0.2f;
    public float m_distance = 2.0f;
    public float m_waitTime = 0.5f;
    public Vector3 m_direction = new Vector3(1.0f, 0.0f, 0.0f);
    public bool m_boomerang = true;
    public bool m_waitForPlayer = false;
    public bool m_returnWithoutPlayer = false;
    public float m_timeWithOutPlayer = 2.0f;

    private float m_distanceTraveled = 0.0f;
    private float m_timeWaited = 0.0f;
    private int m_sense = 1;
    private PlatformOneDirectionState m_state = PlatformOneDirectionState.STOP;
    private bool m_isPlayer = false;
    
	
    // Use this for initialization
	void Start (){}
	
	// Update is called once per frame
	void Update ()
    {
        switch (m_state)
        {
            case PlatformOneDirectionState.STOP:
                if(!m_waitForPlayer || m_isPlayer)
                {
                    m_state = PlatformOneDirectionState.MOVE;
                }
                break;
            case PlatformOneDirectionState.WAIT:
                m_timeWaited += Time.deltaTime;
                if (m_timeWaited >= m_waitTime)
                {
                    m_sense = m_sense == 1 ? -1 : 1;
                    m_state = m_sense == 1 ? PlatformOneDirectionState.STOP : PlatformOneDirectionState.RETURN;
                    m_timeWaited = 0.0f;
                }
                break;
            case PlatformOneDirectionState.RETURN:
                if (m_waitForPlayer && !m_isPlayer)
                {
                    m_timeWaited += Time.deltaTime;
                    if(m_timeWaited >= m_timeWithOutPlayer)
                    {
                        m_state = PlatformOneDirectionState.MOVE;
                    }
                }else
                {
                    if (!m_waitForPlayer)
                        m_state = PlatformOneDirectionState.MOVE;
                    else
                    {
                        m_timeWaited = 0;
                    }
                }
                break;
            case PlatformOneDirectionState.MOVE:
                float distanceToMove = m_distance - m_distanceTraveled;
                if (m_speed * Time.deltaTime <= distanceToMove)
                    distanceToMove = m_speed * Time.deltaTime;
                m_distanceTraveled += distanceToMove;
                transform.Translate(m_sense * m_direction * distanceToMove);
                if (m_distanceTraveled >= m_distance && m_boomerang)
                {
                    m_distanceTraveled = 0;
                    m_state = PlatformOneDirectionState.WAIT;
                }
                break;   
        }        
	}

    private void OnCollisionEnter(Collision colInfo)
    {
        if (colInfo.collider.tag == "Player")
        {
            m_isPlayer = true;
        }
    }

    private void OnCollisionExit(Collision colInfo)
    {
        if (colInfo.collider.tag == "Player")
        {
            m_isPlayer = false;
        }
    }
}
