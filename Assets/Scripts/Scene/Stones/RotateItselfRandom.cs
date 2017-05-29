using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateItselfRandom : MonoBehaviour {

    public float m_rotationSpeed;
    Vector3 m_rotationRandomVector;

	// Use this for initialization
	void Start ()
    {
        m_rotationRandomVector = new Vector3(Random.value, Random.value, Random.value).normalized;
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(m_rotationRandomVector * m_rotationSpeed * Time.deltaTime);
    }
}
