using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum DeclineParticleState
{
    WAIT,
    DECLINE,
    DISABLE,
}

public class DeclineParticle : MonoBehaviour {
    public float m_timeAlive;

    private float m_time;
    private DeclineParticleState m_state = DeclineParticleState.WAIT;
    private List<ParticleSystem> m_particleSystems;
    private ParticleSystem m_thisSystem;

    // Use this for initialization
    void Start () {
        m_time = 0.0f;
        m_particleSystems = new List<ParticleSystem>();
        m_particleSystems.Add(m_thisSystem = GetComponent<ParticleSystem>());
        m_particleSystems.AddRange(GetComponentsInChildren<ParticleSystem>());
	}
	
	// Update is called once per frame
	void Update () {
		switch(m_state)
        {
            case DeclineParticleState.WAIT:
                m_time += Time.deltaTime;
                if(m_time >= m_timeAlive)
                {
                    m_state = DeclineParticleState.DECLINE;
                }
                break;
            case DeclineParticleState.DECLINE:
                foreach(ParticleSystem particle in m_particleSystems)
                {
                    if(particle != null)
                    {
                        ParticleSystem.EmissionModule emission = particle.emission;
                        emission.enabled = false;
                    }
                }
                m_time = 0.0f;
                m_state = DeclineParticleState.DISABLE;
                break;
            case DeclineParticleState.DISABLE:
                m_time += Time.deltaTime;
                if (!m_thisSystem.IsAlive() || m_time >= m_timeAlive)
                {
                    gameObject.SetActive(false);
                    m_time = 0.0f;
                    m_state = DeclineParticleState.WAIT;
                    foreach (ParticleSystem particle in m_particleSystems)
                    {
                        if (particle != null)
                        {
                            ParticleSystem.EmissionModule emission = particle.emission;
                            emission.enabled = true;
                        }
                    }
                }
                    
                break;
        }
	}
}
