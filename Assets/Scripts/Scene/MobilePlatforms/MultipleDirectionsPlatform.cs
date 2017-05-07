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
    RESTART,
}

public class MultipleDirectionsPlatform : MonoBehaviour {
    public bool m_startWithPlayer;
    public float m_speedMove;
    public float m_speedRotate;
    public float m_waitTime;
    public Transform m_initicalTransform;

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
  
    
    private float m_verticalDistance = 10.0f;
    private float m_speedVertical = 4.0f;

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
                float vertical1ToMove = m_verticalDistance - m_distanceTraveled;
                if (m_speedVertical * Time.deltaTime <= vertical1ToMove)
                    vertical1ToMove = m_speedVertical * Time.deltaTime;
                m_distanceTraveled += vertical1ToMove;
                transform.Translate(new Vector3(0.0f, 0.0f, -1.0f) * vertical1ToMove);
                if (m_distanceTraveled >= m_verticalDistance)
                {
                    m_distanceTraveled = 0;
                    transform.position = m_initicalTransform.position;
                    transform.rotation = m_initicalTransform.rotation;
                    m_state = PlatformState.RESTART;
                }
                PlanetGravity.AlignWithFather(gameObject);
                break;
            case PlatformState.RESTART:
                float verticalToMove = m_verticalDistance - m_distanceTraveled;
                if (m_speedVertical * Time.deltaTime <= verticalToMove)
                    verticalToMove = m_speedVertical * Time.deltaTime;
                m_distanceTraveled += verticalToMove;
                transform.Translate(new Vector3(0.0f, 0.0f, 1.0f) * verticalToMove);
                if (m_distanceTraveled >= m_verticalDistance)
                {
                    m_distanceTraveled = 0;
                    m_state = PlatformState.WAIT;
                }
                PlanetGravity.AlignWithFather(gameObject);
                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
            m_playerDetected = true;
    }
}
