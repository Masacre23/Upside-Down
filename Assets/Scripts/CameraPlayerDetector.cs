using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerDetector : MonoBehaviour
{
    public bool m_playerInside { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !m_playerInside)
        {
            m_playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && m_playerInside)
        {
            m_playerInside = false;
        }
    }
}
