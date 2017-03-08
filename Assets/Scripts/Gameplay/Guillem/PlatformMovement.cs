using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour {
    Vector3 m_StartPosition;
    [SerializeField] Vector3 m_VectorMovement;
    [SerializeField] float m_MaxDisplacement;
    [SerializeField] float m_Speed;

	// Use this for initialization
	void Start ()
    {
        m_StartPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(m_VectorMovement * m_Speed * Time.deltaTime);
        if (Vector3.Distance(m_StartPosition, transform.position) > m_MaxDisplacement)
        {
            m_VectorMovement = -m_VectorMovement;
        }
	}
}
