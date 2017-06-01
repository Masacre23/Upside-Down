using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour {

    //public KeyCode mPauseKey;
    public string m_pauseInputButton = "Pause";
    public GameObject mPausePanel;

    public EventSystem m_eventSysterm;

    private GameObject m_selected;

    private Player m_player;

    private bool mIsPaused = false;
	// Use this for initialization
	void Start ()
    {
        mPausePanel.SetActive(mIsPaused);
        m_selected = m_eventSysterm.firstSelectedGameObject;
        m_player = GameObject.Find("Player").GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(mPauseKey))
        if (Input.GetButtonDown(m_pauseInputButton))
        {
            Paused();
            m_player.PausePlayer();
        }
        if (m_eventSysterm.currentSelectedGameObject != m_selected)
        {
            if (m_eventSysterm.currentSelectedGameObject == null)
                m_eventSysterm.SetSelectedGameObject(m_selected);
            else
                m_selected = m_eventSysterm.currentSelectedGameObject;
        }
    }

    void Paused()
    {
        mIsPaused = !mIsPaused;
        mPausePanel.SetActive(mIsPaused);
        Time.timeScale = mIsPaused ? 0 : 1;
        //m_player.m_negatePlayerInput = !m_player.m_negatePlayerInput;
    }

    public void Resume()
    {
        Paused();
        m_player.UnpausePlayer();
    }

    public void Quit()
    {
        Scenes.LoadScene(Scenes.MainMenu);
    }
}
