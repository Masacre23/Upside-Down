using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiting : MonoBehaviour {

    public float m_speed = 1.0f;
    public Vector3 m_rotation = Vector3.up;

	// Use this for initialization
	void Start ()
    {
        m_rotation = m_rotation.normalized;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(m_rotation, m_speed * Time.deltaTime);
    }
}
