using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingOnWater : MonoBehaviour
{
    public GameObject m_prefabEffect;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	void OnTriggerEnter(Collider other)
    {
        Vector3 direction = other.transform.position - transform.position;

        EffectsManager.Instance.GetEffect(m_prefabEffect, other.transform.position, direction.normalized, transform);
    }
}
