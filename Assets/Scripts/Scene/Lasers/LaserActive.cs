using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserActive : MonoBehaviour {

    public GameObject m_prefabEffect;
    public Transform m_transfom;

    private GameObject m_effect;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        int player = LayerMask.NameToLayer("Player");
        if (other.gameObject.layer == player)
        {
            m_effect = EffectsManager.Instance.GetEffect(m_prefabEffect, m_transfom, transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int player = LayerMask.NameToLayer("Player");
        if (other.gameObject.layer == player && m_effect != null)
        {
            m_effect.SetActive(false);   
        }
    }
}
