using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateOnExit : MonoBehaviour
{
    Animator m_animator;
    bool m_isAnimated = true;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

	// Use this for initialization
	void Start ()
    {
        m_animator.SetBool("Animate", m_isAnimated);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_isAnimated = false;
            m_animator.SetBool("Animate", m_isAnimated);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_isAnimated = true;
            m_animator.SetBool("Animate", m_isAnimated);
        }
    }
}
