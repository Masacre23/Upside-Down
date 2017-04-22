using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    private ParticleSystem m_thisSystem;

	// Use this for initialization
	void Start ()
    {
        m_thisSystem = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!m_thisSystem.IsAlive())
            gameObject.SetActive(false);
    }
}
