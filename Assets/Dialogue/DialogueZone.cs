using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueZone : MonoBehaviour
{
    string m_inputNameButton = "Submit";
    public GameObject m_dialogue;

    bool m_playerInside = false;
    Player m_playerManager;
    bool m_alreadyPlayed = false;

    void Awake()
    {
        m_playerManager = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update ()
    {
        //if (Input.GetKeyDown(KeyCode.Return) && playerInside && !b)
        if (!m_alreadyPlayed && m_playerInside && !m_playerManager.m_paused && Input.GetButtonDown(m_inputNameButton))
        {
            m_dialogue.SetActive(true);
            m_playerManager.m_negatePlayerInput = true;
            m_alreadyPlayed = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_playerInside = true;
            if (!m_alreadyPlayed)
            {
                m_playerManager.m_negateJump = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_playerInside = false;
            m_playerManager.m_negateJump = false;
        }    
    }
}
