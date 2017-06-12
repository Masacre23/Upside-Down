using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicFish : MonoBehaviour {

    public Transform m_initialPosition;
    public float m_alpha;
    public float m_initialSpeed;
    public float m_gravity;

    private Vector3 m_initialVector;
    private float m_sinAlpha;
    private float m_cosAlpha;
    private float m_sinBeta;
    private float m_cosBeta;
    private float m_aceleration;
    private float m_lastSpeed;


    // Use this for initialization
    void Start () {
        m_initialVector = m_initialPosition.position;
        m_sinAlpha = Mathf.Sin(Mathf.Deg2Rad * m_alpha);
        m_cosAlpha = Mathf.Cos(Mathf.Deg2Rad * m_alpha);
        m_aceleration = m_gravity / 2;
        m_lastSpeed = m_initialSpeed;
    }
	
	// Update is called once per frame
	void Update () {
		if(m_lastSpeed <= -m_initialSpeed)
        {
            transform.position = m_initialVector;
            m_lastSpeed = m_initialSpeed;
        }
        else
        {
            Vector3 newPosition = Vector3.zero;
            newPosition.x = transform.position.x + m_lastSpeed * m_cosAlpha * Time.deltaTime;
            newPosition.z = transform.position.z;
            newPosition.y = transform.position.y + m_lastSpeed * m_sinAlpha * Time.deltaTime - m_aceleration * Time.deltaTime * Time.deltaTime;
            m_lastSpeed = m_lastSpeed * m_sinAlpha - m_gravity * Time.deltaTime;

            transform.Translate(newPosition -transform.position);
        }
	}
}
