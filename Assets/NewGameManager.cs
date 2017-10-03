using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class NewGameManager : MonoBehaviour {

    public GameObject m_previousPanel;
    public GameObject m_newGamePanel;

    public GameObject m_previousButton;
    public EventSystem m_eventSystem;

    public UISelectedIndicator m_indicator;
    public AudioClip m_bossMusic;

    private UISoundEffects m_sound;
    private GameObject m_selected;

    // Use this for initialization
    void Start()
    {
        m_sound = GetComponent<UISoundEffects>();
        m_selected = m_eventSystem.firstSelectedGameObject;
        m_indicator.gameObject.SetActive(true);
        m_indicator.SelectNewButton(m_selected);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_eventSystem.currentSelectedGameObject != m_selected)
        {
            if (m_eventSystem.currentSelectedGameObject == null)
                m_eventSystem.SetSelectedGameObject(m_selected);
            else
                m_selected = m_eventSystem.currentSelectedGameObject;
            m_indicator.SelectNewButton(m_selected);
            if (m_sound != null)
            {
                m_sound.PlayChange();
            }
        }

        if (CrossPlatformInputManager.GetButtonDown("Cancel"))
        {
            m_newGamePanel.SetActive(false);
            m_previousPanel.SetActive(true);
            m_eventSystem.SetSelectedGameObject(m_previousButton);
        }
    }

    public void StartBossScene()
    {
        AudioManager.Instance().PlayMusic(m_bossMusic, 1.0f);
        Scenes.LoadScene(Scenes.Boss);
    }
}
