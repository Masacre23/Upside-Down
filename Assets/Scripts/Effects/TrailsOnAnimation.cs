using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailsOnAnimation : MonoBehaviour
{

    [System.Serializable]
    struct TrailInfo
    {
        public string m_animationName;
        public Transform m_emitter;
        public float m_timeInit;
        public float m_timeEnd;
        public GameObject m_trailPrefab;

        [HideInInspector] public GameObject m_trailInstance;
        [HideInInspector] public bool m_instantiated;
    }

    [SerializeField] TrailInfo[] m_trails;

    Animator m_animator;

    void Start()
    {
        m_animator = GetComponent<Animator>();

        for (int i = 0; i < m_trails.Length; i++)
        {
            m_trails[i].m_instantiated = false;
            m_trails[i].m_trailInstance = null;
        }
    }

    void Update()
    {
        AnimatorStateInfo currentState = m_animator.GetCurrentAnimatorStateInfo(0);
        float normalizedTime = currentState.normalizedTime;
        normalizedTime -= (int)normalizedTime;

        for (int i = 0; i < m_trails.Length; i++)
        {
            if (currentState.IsName(m_trails[i].m_animationName))
            {
                if (!m_trails[i].m_instantiated)
                {
                    if (normalizedTime > m_trails[i].m_timeInit && normalizedTime < m_trails[i].m_timeEnd)
                    {
                        m_trails[i].m_instantiated = true;
                        m_trails[i].m_trailInstance = EffectsManager.Instance.GetEffect(m_trails[i].m_trailPrefab, m_trails[i].m_emitter);
                    }
                }
                else
                {
                    if (normalizedTime > m_trails[i].m_timeEnd || normalizedTime < m_trails[i].m_timeInit)
                    {
                        m_trails[i].m_instantiated = false;
                        TrailDisableOnCall callDisable = m_trails[i].m_trailInstance.GetComponent<TrailDisableOnCall>();
                        if (callDisable)
                            callDisable.FinishTrail();
                        else
                            m_trails[i].m_trailInstance.SetActive(false);
                        m_trails[i].m_trailInstance = null;
                    }
                }
            }
        }

    }
}
