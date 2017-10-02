using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class CreditsManager : MonoBehaviour {

    public GameObject m_previousPanel;
    public GameObject m_creditsPanel;

    public GameObject m_previousButton;
    public EventSystem m_eventSystem;

    public Scrollbar m_srollBar;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Cancel"))
        {
            m_creditsPanel.SetActive(false);
            m_previousPanel.SetActive(true);
            m_eventSystem.SetSelectedGameObject(m_previousButton);
        }
        float axisHorizontal = Input.GetAxis("Vertical");
        m_srollBar.value += axisHorizontal * 0.1f;
    }
}
