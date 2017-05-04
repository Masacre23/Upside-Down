using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopInputWhileMoving : MonoBehaviour {

    OnedirectionalMobilePlatform m_mainScript;
    Player m_player;

	// Use this for initialization
	void Start ()
    {
        m_mainScript = GetComponent<OnedirectionalMobilePlatform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_player)
        {
            if (m_mainScript.IsMoving())
            {
                m_player.m_negatePlayerInput = true;
            }
            else
            {
                m_player.m_negatePlayerInput = false;
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_player = other.GetComponent<Player>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_player = null;
        }
    }
}
