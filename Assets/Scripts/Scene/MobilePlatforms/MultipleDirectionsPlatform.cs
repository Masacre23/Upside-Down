using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlatformState
{
    WAIT,
    MOVE,
    ROTATE,
    FINISH,
}

public class MultipleDirectionsPlatform : MonoBehaviour {
    public float m_speedMove;
    public float m_speedRotate;
    public float m_waitTime;
  
    public float[] m_distance;
    public Vector3[] m_direction;

    public float[] m_angle;
    public GameObject[] m_center;
    public Vector3[] m_rotation;

    private int m_movementIndex = -1;
    private int m_rotateIndex = -1;
    private float m_distanceTraveled = 0.0f;
    private float m_angleRotated = 0.0f;
    private float m_timeWaited = 0.0f;
    private Vector3 m_speedLastUpdate;
    private PlatformState m_state = PlatformState.WAIT;

    // Use this for initialization
    void Start()
    {
        m_speedLastUpdate = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case PlatformState.WAIT:
                m_timeWaited += Time.deltaTime;
                if (m_timeWaited >= m_waitTime)
                {
                    if (m_movementIndex == m_rotateIndex)
                    {
                        m_movementIndex++;
                        if (m_movementIndex >= m_direction.Length)
                            m_state = PlatformState.FINISH;
                        else
                            m_state = PlatformState.MOVE;
                    }
                    else
                    {
                        m_rotateIndex++;
                        if (m_rotateIndex >= m_rotation.Length)
                            m_state = PlatformState.FINISH;
                        else
                            m_state = PlatformState.ROTATE;
                    }
                    m_timeWaited = 0.0f;
                }
                m_speedLastUpdate = Vector3.zero;
                break;
            case PlatformState.MOVE:
                if(m_movementIndex < m_direction.Length)
                {
                    float distanceToMove = m_distance[m_movementIndex] - m_distanceTraveled;
                    if (m_speedMove * Time.deltaTime <= distanceToMove)
                        distanceToMove = m_speedMove * Time.deltaTime;
                    m_distanceTraveled += distanceToMove;
                    transform.Translate( m_direction[m_movementIndex] * distanceToMove);
                    if (m_distanceTraveled >= m_distance[m_movementIndex])
                    {
                        m_distanceTraveled = 0;
                        //m_sense = m_sense == 1 ? -1 : 1;
                        m_state = PlatformState.WAIT;
                    }
                    m_speedLastUpdate =  m_direction[m_movementIndex] * m_speedMove;
                    PlanetGravity.AlignWithFather(gameObject);
                }
                else
                {
                    m_state = PlatformState.WAIT;
                }
                break;
            case PlatformState.ROTATE:
                if (m_rotateIndex < m_rotation.Length)
                {
                    float angleToRotate = m_angle[m_rotateIndex] - m_angleRotated;
                    if (m_speedRotate * Time.deltaTime <= angleToRotate)
                        angleToRotate = m_speedRotate * Time.deltaTime;
                    m_angleRotated += angleToRotate;
                    //transform.Rotate(m_rotation[m_rotateIndex] * angleToRotate);
                    Transform parent = transform.parent;
                    transform.parent = m_center[m_rotateIndex].transform;
                    m_center[m_rotateIndex].transform.Rotate(m_rotation[m_rotateIndex] * angleToRotate);
                    //transform.RotateAround(m_center[m_rotateIndex].transform.position, new Vector3(0, 0, 1.0f), angleToRotate);
                    if (m_angleRotated >= m_angle[m_rotateIndex])
                    {
                        m_angleRotated = 0;
                        //m_sense = m_sense == 1 ? -1 : 1;
                        m_state = PlatformState.WAIT;
                    }
                    m_speedLastUpdate = m_direction[m_movementIndex] * m_speedRotate;
                    transform.parent = parent;
                    PlanetGravity.AlignWithFather(gameObject);
                }
                else
                {
                    m_state = PlatformState.WAIT;
                }
                break;
        }
    }
}
