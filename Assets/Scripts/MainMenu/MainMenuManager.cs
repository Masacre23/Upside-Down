using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour {
	public EventSystem m_eventSysterm;

    private GameObject m_selected;

    private void Start()
    {
        m_selected = m_eventSysterm.firstSelectedGameObject;
    }

    private void Update()
    {
        if (m_eventSysterm.currentSelectedGameObject != m_selected)
        {
            if (m_eventSysterm.currentSelectedGameObject == null)
                m_eventSysterm.SetSelectedGameObject(m_selected);
            else
                m_selected = m_eventSysterm.currentSelectedGameObject;
        }
    }

    public void PressPlayButton(){
        Scenes.LoadScene(Scenes.Level1);
	}

	public void PreesQuitButton(){
		Debug.Log("Quit");
		Application.Quit();
	}

}
