using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GeyserState
{
    STOP,
    TROWING,
    WAIT,
}
public class Geyser : MonoBehaviour {
    public Transform[] m_transforms;
    public GameObject m_prefabEffect;
    public TriggerPlatform m_trigger;

    public float m_timeBetweenGeyser = 0.2f;
    public float m_timeWait = 1.0f;

    private GeyserState m_state;
    private int m_nextIndex = 0;
    private float m_timeLastGyser;
    private float m_timeWaiting;

    // Use this for initialization
    void Start () {
        m_state = GeyserState.STOP;
	}
	
	// Update is called once per frame
	void Update () {
		switch(m_state)
        {
            case GeyserState.STOP:
                if (m_trigger.m_playerDetected)
                {
                    m_state = GeyserState.TROWING;
                    m_nextIndex = 0;
                    m_timeLastGyser = m_timeBetweenGeyser;
                }
                break;
            case GeyserState.TROWING:
                m_timeLastGyser += Time.deltaTime;
                if(m_timeLastGyser > m_timeBetweenGeyser)
                {
                    m_timeLastGyser = 0.0f;
                    EffectsManager.Instance.GetEffect(m_prefabEffect, m_transforms[m_nextIndex], transform);
                    m_nextIndex++;
                    if(m_nextIndex == m_transforms.Length)
                    {
                        m_timeWaiting = 0.0f;
                        m_state = GeyserState.WAIT;
                    }
                }
                break;
            case GeyserState.WAIT:
                m_timeWaiting += Time.deltaTime;
                if(m_timeWaiting >= m_timeWait)
                {
                    m_state = GeyserState.TROWING;
                    m_nextIndex = 0;
                    m_timeLastGyser = m_timeBetweenGeyser;
                }
                break;
        }
	}
}
