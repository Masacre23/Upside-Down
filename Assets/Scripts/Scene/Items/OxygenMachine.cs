using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenMachine : MonoBehaviour
{
    string m_inputNameButton = "Activate";
    bool m_playerInside = false;
    Player m_playerManager;

    void Awake()
    {
        m_playerManager = GameObject.Find("Player").GetComponent<Player>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_playerInside && Input.GetButtonDown(m_inputNameButton))
        {
            m_playerManager.m_oxigen.SetOxygenMax();
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            m_playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            m_playerInside = false;
    }
}
