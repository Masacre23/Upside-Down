using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxigenPlayer : MonoBehaviour
{

    public float m_maxOxigen = 100.0f;
    public float m_oxigen = 0.0f;

    private float m_lostOxigen = 0;
    private float m_gainedOxygen = 0;

    private float m_downSpeed = 10.0f;
    private float m_upSpeed = 30.0f;

    // Use this for initialization
    void Start ()
    {
        m_oxigen = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        //m_oxigen -= Time.deltaTime;
        if (m_lostOxigen > 0)
        {
            m_oxigen -= m_downSpeed * Time.deltaTime;
            m_lostOxigen-= m_downSpeed * Time.deltaTime;
            if(m_lostOxigen < 0)
            {
                m_oxigen -= m_lostOxigen;
                m_lostOxigen = 0;
            }
        }
        if (m_gainedOxygen > 0)
        {
            m_oxigen += m_upSpeed * Time.deltaTime;
            m_gainedOxygen -= m_upSpeed * Time.deltaTime;
            if (m_gainedOxygen < 0)
            {
                m_oxigen += m_gainedOxygen;
                m_gainedOxygen = 0;
            }
        }
        if (m_oxigen < 0.0f)
            m_oxigen = 0.0f;
        if (m_oxigen > m_maxOxigen)
            m_oxigen = m_maxOxigen;

        HUDManager.ChangeOxigen(m_oxigen / m_maxOxigen);
    }

    public void LostOxigen(float oxigen)
    {
        m_lostOxigen = oxigen;
    }

    public void RecoverOxygen(float oxigen)
    {
        m_gainedOxygen = oxigen;
    }

    public void SetOxygenMax()
    {
        m_lostOxigen = 0.0f;
        m_gainedOxygen = m_maxOxigen;
    }

    public bool HasEnoughOxygen(float oxygenCost)
    {
        return !(m_oxigen - oxygenCost < 0) || Mathf.Approximately(m_oxigen - oxygenCost, 0.0f);
    }
}
