using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class PauseMenuManager : MonoBehaviour {

    //public KeyCode mPauseKey;
    public string m_pauseInputButton = "Pause";
    public GameObject mPausePanel;
    public GameObject m_optionsPanel;
    public GameObject m_creditsPanel;
    public EventSystem m_eventSysterm;
    public AudioClip m_mainMenuClip;

    public UISelectedIndicator m_indicator;
    private GameObject m_selected;

    private Player m_player;

    private bool mIsPaused = false;
	// Use this for initialization
	void Start ()
    {
        mPausePanel.SetActive(mIsPaused);
        m_selected = m_eventSysterm.firstSelectedGameObject;
        m_player = GameObject.Find("Player").GetComponent<Player>();
        m_indicator.SelectNewButton(m_selected);
    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(mPauseKey))
        if (CrossPlatformInputManager.GetButtonDown(m_pauseInputButton))
        {
            if (!m_optionsPanel.activeSelf && !m_creditsPanel.activeSelf)
            {
                Paused();
                if (mIsPaused)
                    m_player.PausePlayer();
                else
                    m_player.UnpausePlayer();
            }
        }
        if (m_eventSysterm.currentSelectedGameObject != m_selected)
        {
            if (m_eventSysterm.currentSelectedGameObject == null)
                m_eventSysterm.SetSelectedGameObject(m_selected);
            else
                m_selected = m_eventSysterm.currentSelectedGameObject;
            m_indicator.SelectNewButton(m_selected);
        }
    }

    void Paused()
    {
        mIsPaused = !mIsPaused;
        mPausePanel.SetActive(mIsPaused);
        m_player.PausePlayer();
    }

    public void Resume()
    {
        Paused();
        m_player.UnpausePlayer();
    }

    public void Quit()
    {
        Paused();
        if (AudioManager.Instance())
        {
            AudioManager.Instance().PlayMusic(m_mainMenuClip, 1.0f);
        }
        Scenes.LoadScene(Scenes.MainMenu);
    }
}
