using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour {
	public EventSystem m_eventSysterm;
    public GameObject m_canvasLogo;
    public GameObject m_canvasMenu;
    public GameObject m_panelButtons;
    public GameObject m_panelAnyKey;
    public UISelectedIndicator m_selectedMasck;
    public VideoPlayer m_video;

    private UISoundEffects m_sound;
    private GameObject m_selected;
    private bool showLogo = true;
    private bool waitAnyKey = false;
    private float time = 0.0f;

    private static bool firstTime = true;
	public AsyncOperation async;
    private void Start()
    {
        Cursor.visible = false;
        m_sound = GetComponent<UISoundEffects>();
        m_selected = m_eventSysterm.firstSelectedGameObject;
        showLogo = firstTime;
        m_canvasLogo.SetActive(showLogo);
        m_canvasMenu.SetActive(!showLogo);
        m_panelButtons.SetActive(!showLogo);
        waitAnyKey = firstTime;
        //m_panelAnyKey.SetActive(waitAnyKey);
        firstTime = false;
		async = Scenes.LoadSceneAsync (Scenes.Level1);
		async.allowSceneActivation = false;
    }

    private void Update()
    {
        if (showLogo)
        {
            time += Time.deltaTime;
			if (time >= 5.0f)
			{
				m_canvasLogo.SetActive (false);
			}

			if (time >= 21.0f) 
			{
				m_canvasMenu.SetActive(true);
			}

			if (time >= 23.0f) 
			{
				m_panelAnyKey.SetActive (true);
				m_eventSysterm.SetSelectedGameObject(null);
				showLogo = false;
			}
        }
        else if (waitAnyKey)
        {
            if (Input.anyKey)
            {
                waitAnyKey = false;
                m_panelAnyKey.SetActive(false);
                m_panelButtons.SetActive(true);
                m_selectedMasck.gameObject.SetActive(true);
                m_selectedMasck.SelectNewButton(m_selected);
            }
        }
        else
        {
            if (m_canvasMenu.activeSelf)
            {
                if (m_eventSysterm.currentSelectedGameObject != m_selected)
                {
                    if (m_eventSysterm.currentSelectedGameObject == null)
                        m_eventSysterm.SetSelectedGameObject(m_selected);
                    else
                        m_selected = m_eventSysterm.currentSelectedGameObject;
                    m_selectedMasck.SelectNewButton(m_selected);
                    if (m_sound != null)
                    {
                        m_sound.PlayChange();
                    }
                }
            }
        }
        
    }

    public void PressPlayButton(){
        m_video.PlayVideo();
	}

	public void PreesQuitButton(){
		Application.Quit();
	}

}
