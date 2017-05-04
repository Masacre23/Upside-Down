using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWalls : MonoBehaviour {

    [SerializeField] float m_timeUntilActivation = 1.0f;
    [SerializeField] float m_timeUntilDesactivation = 5.0f;
    [SerializeField] GameObject m_walls;
    Player m_player;
    float m_time;

	// Use this for initialization
	void Start ()
    {
        m_walls.SetActive(false);
    }
	
    void Reset()
    {
        m_walls.SetActive(false);
    }

	// Update is called once per frame
	void Update ()
    {
        if (m_player)
        {
            m_time += Time.deltaTime;
            if (m_time > m_timeUntilActivation)
            {
                m_walls.SetActive(true);
                m_time = 0.0f;
            }
        }
        else
        {
            m_time += Time.deltaTime;
            if (m_time > m_timeUntilDesactivation)
            {
                m_walls.SetActive(false);
                m_time = 0.0f;
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_player = other.GetComponent<Player>();
            m_time = 0.0f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_player = null;
            m_time = 0.0f;
        }
    }
}
