using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsMotion : MonoBehaviour {
    public readonly int m_maxAnimations = 3;

    private Animator m_animator;
    private int m_currentAnimation = 0;
    private float m_timeToChange = 5.0f;
    private float m_time = 0.0f;
	
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

	// Update is called once per frame
	void Update () {
        m_time += Time.deltaTime;
        if(m_time >= m_timeToChange)
        {
            m_time = 0;
            m_timeToChange = Random.Range(5.0f, 10.0f);
            int previousAnimation = m_currentAnimation;
            m_currentAnimation = Random.Range(0, m_maxAnimations);
            if (previousAnimation != m_currentAnimation) {
                m_animator.SetInteger("DanceIndex", m_currentAnimation);
                m_animator.SetBool("ChangeAnimation", true);
            }
        }
	}
}
