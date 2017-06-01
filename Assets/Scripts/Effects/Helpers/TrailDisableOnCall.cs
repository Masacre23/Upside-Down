using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailDisableOnCall : MonoBehaviour {

    TrailRenderer m_trailRenderer;
    Transform m_parentDisabled;
    bool m_disabledTail = false;
    float m_timeDeactivating = 0.0f;

    void Start()
    {
        m_trailRenderer = GetComponent<TrailRenderer>();
        m_parentDisabled = EffectsManager.Instance.GetDefaultParent();
    }

    void LateUpdate()
    {
        if (m_disabledTail)
        {
            m_timeDeactivating += Time.deltaTime;
            if (m_timeDeactivating > m_trailRenderer.time)
            {
                m_timeDeactivating = 0.0f;
                m_disabledTail = false;
                gameObject.SetActive(false);
            }
        }
    }

    public void FinishTrail()
    {
        m_trailRenderer.transform.parent = m_parentDisabled;
        m_disabledTail = true;
    }
}
