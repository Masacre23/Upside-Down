using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlatform : MonoBehaviour {

    public bool m_playerDetected = false;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            m_playerDetected = true;
        }
    }

}
