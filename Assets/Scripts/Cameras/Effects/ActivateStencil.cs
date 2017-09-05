using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateStencil : MonoBehaviour
{
    public Transform m_raycastTarget;
    public GameObject m_stencilSphere;

    public bool m_stencilActive = false;

    public LayerMask m_meshesToCheck;

	// Use this for initialization
	void Start ()
    {
        SetStencil(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 toTarget = m_raycastTarget.position - transform.position;
        bool hasTarget = Physics.Raycast(transform.position, toTarget.normalized, toTarget.magnitude, m_meshesToCheck);

        if (!m_stencilActive && hasTarget)
            SetStencil(true);
        else if (m_stencilActive && !hasTarget)
            SetStencil(false);
	}

    private void SetStencil(bool value)
    {
        if (m_stencilSphere)
            m_stencilSphere.SetActive(value);

        m_stencilActive = value;
    }
}
