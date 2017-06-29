using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DissapearOnContact : MonoBehaviour
{

    public float m_growthRate = 0.2f;
    public TriggerPlatform m_trigger;

    private Vector3 m_scale;

    private bool m_running = true;

    // Use this for initialization
    void Start()
    {
        m_scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_running)
        {
            if (m_trigger.m_playerDetected)
            {
                m_scale -= (new Vector3(1.0f, 1.0f, 1.0f) * m_growthRate * Time.deltaTime);
                transform.localScale = m_scale;
                if (m_scale.x < 0.0f)
                {
                    transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    m_running = false;
                }
            }

        }
    }
}
