using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxigenPlayer : MonoBehaviour
{

    public float m_maxOxigen = 240;
    public float m_oxigen = 240;

    private float m_lostOxigen = 0;
    // Use this for initialization
    void Start () {
        m_oxigen = 240.0f;
	}
	
	// Update is called once per frame
	void Update () {
        //m_oxigen -= Time.deltaTime;
        if(m_lostOxigen > 0)
        {
            m_oxigen -= 10 * Time.deltaTime;
            m_lostOxigen-= 10 * Time.deltaTime;
            if(m_lostOxigen < 0)
            {
                m_oxigen -= m_lostOxigen;
                m_lostOxigen = 0;
            }
        }
        if (m_oxigen < 0.0f)
            m_oxigen = 0.0f;

        HUDManager.ChangeOxigen(m_oxigen / m_maxOxigen);
    }

    public void LostOxigen(float oxigen)
    {
        m_lostOxigen = oxigen;
    }

    public bool HasEnoughOxygen(float oxygenCost)
    {
        return m_oxigen > oxygenCost;
    }
}
