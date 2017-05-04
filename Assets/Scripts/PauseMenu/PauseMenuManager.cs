using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour {

    public KeyCode mPauseKey;
    public GameObject mPausePanel;

    public EventSystem m_eventSysterm;

    private GameObject m_selected;

    private bool mIsPaused = false;
	// Use this for initialization
	void Start () {
        mPausePanel.SetActive(mIsPaused);
        m_selected = m_eventSysterm.firstSelectedGameObject;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(mPauseKey)){
            Paused();
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
    }

    public void Resume()
    {
        Paused();
    }

    public void Quit()
    {
        Scenes.LoadScene(Scenes.MainMenu);
    }
}
