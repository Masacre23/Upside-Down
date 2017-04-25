using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableDeathArea : MonoBehaviour {

    [SerializeField] GameObject m_object;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_object.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_object.SetActive(true);
        }
    }
}
