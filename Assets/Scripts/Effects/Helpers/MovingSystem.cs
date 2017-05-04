using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSystem : MonoBehaviour {
    public Vector3 m_direction;
    public float m_speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(m_direction * m_speed * Time.deltaTime);
	}
}
