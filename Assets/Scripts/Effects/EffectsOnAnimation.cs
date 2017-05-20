using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsOnAnimation : MonoBehaviour
{

    [System.Serializable]
    struct ParticleInfo
    {
        public string m_animationName;
        public Transform m_emitter;
        public float m_timeIntantiate;
        public GameObject m_effectPrefab;
        public bool m_onParent;

        [HideInInspector]
        public bool m_instantiated;
        [HideInInspector]
        public float m_timeInstantiated;
    }

    [SerializeField] ParticleInfo[] m_effects;

    Animator m_animator;

    void Start()
    {
        m_animator = GetComponent<Animator>();

        for (int i = 0; i < m_effects.Length; i++)
        {
            m_effects[i].m_instantiated = false;
            m_effects[i].m_timeInstantiated = 0.0f;
        }
    }

    void Update()
    {
        AnimatorStateInfo currentState = m_animator.GetCurrentAnimatorStateInfo(0);
        float normalizedTime = currentState.normalizedTime;
        normalizedTime -= (int)normalizedTime;

        for (int i = 0; i < m_effects.Length; i++)
        {
            if (currentState.IsName(m_effects[i].m_animationName))
            {
                if (!m_effects[i].m_instantiated)
                {
                    if (normalizedTime > m_effects[i].m_timeIntantiate)
                    {
                        if (m_effects[i].m_onParent)
                            EffectsManager.Instance.GetEffect(m_effects[i].m_effectPrefab, m_effects[i].m_emitter);
                        else
                            EffectsManager.Instance.GetEffect(m_effects[i].m_effectPrefab, m_effects[i].m_emitter.position);

                        m_effects[i].m_instantiated = true;
                        m_effects[i].m_timeInstantiated = normalizedTime;
                    }
                }
                else
                {
                    if (m_effects[i].m_timeInstantiated > normalizedTime)
                    {
                        m_effects[i].m_instantiated = false;
                    }
                }
            }
        }

    }
}
