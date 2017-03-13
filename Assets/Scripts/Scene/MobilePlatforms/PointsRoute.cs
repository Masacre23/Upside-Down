using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsRoute : MonoBehaviour {

    public GameObject m_route;
    public bool m_return;
    public float m_speed = 0.1f;

    int m_currentPoint;
    Transform[] m_routePoints;
    Rigidbody m_rigidBody;

	// Use this for initialization
	void Start ()
    {
        m_rigidBody = GetComponent<Rigidbody>();

        Vector3 offset = transform.position - m_route.transform.GetChild(0).position;

        int numberOfPoints = m_route.transform.childCount;
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
            m_routePoints[i].position = m_routePoints[i].position + m_routePoints[i].TransformDirection(offset);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position != m_routePoints[m_currentPoint].position)
        {
            Vector3 position = Vector3.MoveTowards(transform.position, m_routePoints[m_currentPoint].position, m_speed * Time.deltaTime);
            m_rigidBody.MovePosition(position);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, m_routePoints[m_currentPoint].rotation, 10 * m_speed * Time.deltaTime);
            m_rigidBody.MoveRotation(rotation);
        }
        else
            m_currentPoint = (m_currentPoint + 1) % m_routePoints.Length;

    }
}
