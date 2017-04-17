using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlatformState
{
    STOP,
    WAIT,
    MOVE,
    ROTATE,
    FINISH,
}

public class MultipleDirectionsPlatform : MonoBehaviour {
    public bool m_startWithPlayer;
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
    private PlatformState m_state = PlatformState.STOP;
    private bool m_playerDetected = false;
    private Vector3 m_initialPosition;
    private Quaternion m_initialRotation; 

    // Use this for initialization
    void Start()
    {
        m_speedLastUpdate = Vector3.zero;
        m_initialPosition = gameObject.transform.position;
        m_initialRotation = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case PlatformState.STOP:
                if (!m_startWithPlayer)
                    m_state = PlatformState.WAIT;
                else
                {
                    if (m_playerDetected)
                    {
                        m_movementIndex++;
                        if (m_movementIndex >= m_direction.Length)
                            m_state = PlatformState.FINISH;
                        else
                            m_state = PlatformState.MOVE;
                    }
                }
                break;
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
                    Transform parent = transform.parent;
                    transform.parent = m_center[m_rotateIndex].transform;
                    m_center[m_rotateIndex].transform.Rotate(m_rotation[m_rotateIndex] * angleToRotate);
                    if (m_angleRotated >= m_angle[m_rotateIndex])
                    {
                        m_angleRotated = 0;
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
            case PlatformState.FINISH:
                m_movementIndex = -1;
                m_rotateIndex = -1;
                transform.position = m_initialPosition;
                transform.rotation = m_initialRotation;
                m_state = PlatformState.WAIT;
                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
            m_playerDetected = true;
    }
}
