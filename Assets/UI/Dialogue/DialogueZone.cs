using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueZone : MonoBehaviour
{
    string m_inputNameButton = "Activate";
    public GameObject m_dialogue;
	public GameObject m_buttonA;

    bool m_playerInside = false;
    Player m_playerManager;
    bool m_alreadyPlayed = false;

    Dialogue m_currentDialogue;

	public bool instantPlay = false;

    void Awake()
    {
        m_playerManager = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update ()
    {
        //if (Input.GetKeyDown(KeyCode.Return) && playerInside && !b)
		if (!m_alreadyPlayed && m_playerInside && !m_playerManager.m_paused && (Input.GetButtonDown(m_inputNameButton) || instantPlay))
        {
            m_dialogue.SetActive(true);
			//m_dialogue.GetComponent<Dialogue> ().StopAllCoroutines ();
			//m_dialogue.GetComponent<Dialogue> ().Start ();
			m_buttonA.SetActive (false);
            m_playerManager.m_negatePlayerInput = true;
            m_alreadyPlayed = true;
        }

        if (m_alreadyPlayed && m_currentDialogue)
        {
            if (m_currentDialogue.DialogueHasEnded())
            {
                enabled = false;
            }
        }
    }

    public void SetNewDialog(GameObject newDialogue)
    {
        m_alreadyPlayed = false;
        m_dialogue = newDialogue;
        m_currentDialogue = m_dialogue.GetComponentInChildren<Dialogue>();
    }

    private void OnTriggerEnter(Collider other)
    {
		if (other.tag == "Player") 
		{
			m_buttonA.SetActive (true);
			m_playerInside = true;
		}
    }

    private void OnTriggerExit(Collider other)
    {
		if (other.tag == "Player") 
		{
			m_buttonA.SetActive (false);
			m_playerInside = false; 
		}
    }
}
