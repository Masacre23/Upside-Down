using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsManager : MonoBehaviour {

    public ScalableMobilePlatform[] m_clouds;
    public float m_frecuency = 1.0f;
    public float m_period = 8.0f;

    private float m_time = 0;
    private int m_newCloud = 0;
    private bool m_on = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        m_time += Time.deltaTime;
        if (m_on)
        {
            if(m_time > m_frecuency)
            {
                m_clouds[m_newCloud].StartGrowing();
                ++m_newCloud;
                m_time = 0;
                if(m_newCloud >= m_clouds.Length)
                {
                    m_on = false;
                    m_newCloud = 0;
                }
            }
        }
        else
        {
            if(m_time > m_period)
            {
                m_on = true;
                m_time = 0;
            }
        }
	}
}
