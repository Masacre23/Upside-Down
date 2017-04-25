using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour {
	public EventSystem m_eventSysterm;
    public GameObject m_canvasLogo;
    public GameObject m_canvasMenu;

    private GameObject m_selected;
    private bool showLogo = true;
    private float time = 0.0f;

    private void Start()
    {
        m_selected = m_eventSysterm.firstSelectedGameObject;
        showLogo = true;
    }

    private void Update()
    {
        if (showLogo)
        {
            time += Time.deltaTime;
            if(time >= 3.0f)
            {
                m_eventSysterm.SetSelectedGameObject(null);
                showLogo = false;
                m_canvasLogo.SetActive(false);
                m_canvasMenu.SetActive(true);
            }
        }
        else
        {
            if (m_eventSysterm.currentSelectedGameObject != m_selected)
            {
                if (m_eventSysterm.currentSelectedGameObject == null)
                    m_eventSysterm.SetSelectedGameObject(m_selected);
                else
                    m_selected = m_eventSysterm.currentSelectedGameObject;
            }
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
