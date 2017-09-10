using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StencilManager : MonoBehaviour
{
    enum StencilMaskState
    {
        OFF,
        ON,
        INCREASING,
        DECREASING
    }

    public GameObject m_stencilTarget;
    public GameObject m_stencilMask;
    public GameObject m_stencilDetector;
    public StencilArea m_stencilArea;
    public GameObject m_mainCam;

    public float m_timeIncreasingSize;
    public float m_timeDecreasingSize;
    public float m_sizeMin = 0.0f;
    public float m_sizeMax = 5.0f;
    float m_maxTime;
    float m_time;

    LayerMask m_detectorLayerMask;
    [SerializeField] StencilMaskState m_state = StencilMaskState.OFF;

	// Use this for initialization
	void Start ()
    {
        m_state = StencilMaskState.OFF;
        m_stencilMask.SetActive(false);

        m_detectorLayerMask = 1 << LayerMask.NameToLayer("StencilDetector");
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (m_state)
        {
            case StencilMaskState.OFF:
                if (m_stencilArea.m_targetInArea && DetectorBetweenCamPlayer())
                {
                    m_time = 0.0f;
                    m_stencilMask.transform.localScale = Vector3.one * m_sizeMin;
                    m_stencilMask.SetActive(true);
                    m_maxTime = m_timeIncreasingSize;
                    m_state = StencilMaskState.INCREASING;
                }
                break;
            case StencilMaskState.ON:
                if (DetectorBetweenCamPlayer())
                {
                    if (!m_stencilArea.m_targetInArea)
                    {
                        m_time = 0.0f;
                        m_maxTime = m_timeDecreasingSize;
                        m_state = StencilMaskState.DECREASING;
                    }
                }
                else
                {
                    m_stencilMask.SetActive(false);
                    m_state = StencilMaskState.OFF;
                }
                break;
            case StencilMaskState.INCREASING:
                m_time += Time.deltaTime;
                if (DetectorBetweenCamPlayer())
                {
                    if (m_stencilArea.m_targetInArea)
                    {
                        if (m_time >= m_maxTime)
                        {
                            m_stencilMask.transform.localScale = Vector3.one * m_sizeMax;
                            m_state = StencilMaskState.ON;
                        }
                        else
                        {
                            float scale = m_sizeMin + (m_sizeMax - m_sizeMin) * (m_time / m_maxTime);
                            m_stencilMask.transform.localScale = Vector3.one * scale;
                        }
                    }
                    else
                    {
                        float scale = m_sizeMin + (m_sizeMax - m_sizeMin) * (m_time / m_maxTime);
                        m_stencilMask.transform.localScale = Vector3.one * scale;

                        m_maxTime = m_timeDecreasingSize * m_time / m_maxTime;
                        m_time = 0.0f;
                        m_state = StencilMaskState.DECREASING;
                    }
                }
                else
                {
                    m_stencilMask.SetActive(false);
                    m_state = StencilMaskState.OFF;
                }
                break;
            case StencilMaskState.DECREASING:
                m_time += Time.deltaTime;
                if (DetectorBetweenCamPlayer())
                {
                    if (!m_stencilArea.m_targetInArea)
                    {
                        if (m_time >= m_maxTime)
                        {
                            m_stencilMask.transform.localScale = Vector3.one * m_sizeMin;
                            m_stencilMask.SetActive(false);
                            m_state = StencilMaskState.OFF;
                        }
                        else
                        {
                            float scale = m_sizeMin + (m_sizeMax - m_sizeMin) * (1.0f - m_time / m_maxTime);
                            m_stencilMask.transform.localScale = Vector3.one * scale;
                        }
                    }
                    else
                    {
                        float scale = m_sizeMin + (m_sizeMax - m_sizeMin) * (1.0f - m_time / m_maxTime);
                        m_stencilMask.transform.localScale = Vector3.one * scale;

                        m_maxTime = m_timeIncreasingSize * m_time / m_maxTime;
                        m_time = 0.0f;
                        m_state = StencilMaskState.INCREASING;
                    }
                }
                else
                {
                    m_stencilMask.SetActive(false);
                    m_state = StencilMaskState.OFF;
                }
                break;
        }
    }

    private bool DetectorBetweenCamPlayer()
    {
        Vector3 toTarget = m_stencilTarget.transform.position - m_mainCam.transform.position;

        RaycastHit hitInfo;
        if (Physics.Raycast(m_mainCam.transform.position, toTarget.normalized, out hitInfo, toTarget.magnitude, m_detectorLayerMask))
        {
            if (hitInfo.collider.gameObject == m_stencilDetector)
            {
                return true;
            }
        }

        return false;
    }
}
