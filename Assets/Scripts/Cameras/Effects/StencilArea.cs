using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StencilArea : MonoBehaviour
{
    public GameObject m_target;
    public bool m_targetInArea { get; private set; }

    void Start()
    {
        if (!GetComponent<Collider>())
            m_targetInArea = true;
        else
            m_targetInArea = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == m_target)
        {
            m_targetInArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == m_target)
        {
            m_targetInArea = false;
        }
    }
}
