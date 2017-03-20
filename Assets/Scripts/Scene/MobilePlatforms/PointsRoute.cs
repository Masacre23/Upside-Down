using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsRoute : MonoBehaviour {

    public GameObject m_route;
    public bool m_return;
    public bool m_stopOnBoundaries = false;
    public float m_waitOnBoundaries = 2.0f;
    public float m_speed = 0.1f;

    int m_currentPoint;
    Transform[] m_routePoints;
    Rigidbody m_rigidBody;

    float m_distance = 0.0f;
    float m_time = 0.0f;
    float m_timeWaiting = 0.0f;
    Vector3 m_oldPosition;
    Quaternion m_oldRotation;

    int m_boundStart = 0;
    int m_boundEnd = 0;

	// Use this for initialization
	void Start ()
    {
        m_rigidBody = GetComponent<Rigidbody>();

        Vector3 offset = transform.position - m_route.transform.GetChild(0).position;
        offset = m_route.transform.GetChild(0).InverseTransformDirection(offset);

        int numberOfPoints = m_route.transform.childCount;
        m_boundStart = 0;
        m_boundEnd = numberOfPoints - 1;
        if (m_return)
            numberOfPoints = 2 * numberOfPoints - 2;
        m_routePoints = new Transform[numberOfPoints];
        for (int i = 0; i < m_route.transform.childCount; i++)
        {
            m_routePoints.SetValue(m_route.transform.GetChild(i), i);
        }
            
        if (m_return)
        {
            int initialPointBack = m_route.transform.childCount - 2;
            for (int i = initialPointBack; i > 0; i--)
            {
                m_routePoints.SetValue(m_route.transform.GetChild(i), m_route.transform.childCount + initialPointBack - i);
            }    
        }

        for (int i = 0; i < m_route.transform.childCount; i++)
        {
            m_routePoints[i].Translate(offset);
        }

        m_currentPoint = 0;
        m_oldPosition = transform.position;
        m_oldRotation = transform.rotation;
        m_distance = Vector3.Magnitude(m_routePoints[m_currentPoint].position - transform.position);
        m_time = 0.0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position != m_routePoints[m_currentPoint].position)
        {
            float frac = m_speed * m_time / m_distance;
            m_rigidBody.MovePosition(Vector3.Lerp(m_oldPosition, m_routePoints[m_currentPoint].position, frac));
            m_rigidBody.MoveRotation(Quaternion.Slerp(m_oldRotation, m_routePoints[m_currentPoint].rotation, frac));
            m_time += Time.deltaTime;
        }
        else
        {
            if (m_stopOnBoundaries && (m_currentPoint == m_boundStart || m_currentPoint == m_boundEnd))
            {
                if (m_timeWaiting > m_waitOnBoundaries)
                {
                    m_currentPoint = (m_currentPoint + 1) % m_routePoints.Length;

                    m_oldPosition = transform.position;
                    m_oldRotation = transform.rotation;
                    m_distance = Vector3.Magnitude(m_routePoints[m_currentPoint].position - transform.position);
                    m_time = 0.0f;
                    m_timeWaiting = 0.0f;
                }
                else
                {
                    m_timeWaiting += Time.deltaTime;
                }
            }
            else
            {
                m_currentPoint = (m_currentPoint + 1) % m_routePoints.Length;

                m_oldPosition = transform.position;
                m_oldRotation = transform.rotation;
                m_distance = Vector3.Magnitude(m_routePoints[m_currentPoint].position - transform.position);
                m_time = 0.0f;
            }       
        }
            

    }
}
