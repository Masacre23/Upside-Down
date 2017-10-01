using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class OptionsManager : MonoBehaviour {

    public GameObject m_previousPanel;
    public GameObject m_optionsPanel;
    public GameObject m_buttonsPanel;
    public GameObject m_controllerPanel;

    public GameObject m_previousButton;
    public GameObject m_controllerButton;
    public EventSystem m_eventSystem;

    public Slider m_musicVolume;
    public Slider m_effectsVolume;
    public Toggle m_language;

    public UISelectedIndicator m_indicator;

    private UISoundEffects m_sound;
    private GameObject m_selected;
    private bool m_musicSelected = false;
    private bool m_effectsSelected = false;

	// Use this for initialization
	void Start () {
        AudioManager audio = AudioManager.Instance();
        if (audio != null)
        {
            m_musicVolume.value = audio.m_musicVolume;
            m_effectsVolume.value = audio.m_soundVolume;
        }
        m_sound = GetComponent<UISoundEffects>();
        m_musicSelected = false;
        m_effectsSelected = false;
        m_selected = m_eventSystem.firstSelectedGameObject;
        m_indicator.SelectNewButton(m_selected);
    }
	
	// Update is called once per frame
	void Update () {
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

        float axisHorizontal = Input.GetAxis("Horizontal");
        if (m_musicSelected)
        {
            m_musicVolume.value += axisHorizontal * 0.1f;
        }
        if (m_effectsSelected)
        {
            m_effectsVolume.value += axisHorizontal * 0.1f;
        }

        if (CrossPlatformInputManager.GetButtonDown("Cancel"))
        {
            if (m_controllerPanel.activeSelf)
            {
                m_controllerPanel.SetActive(false);
                m_buttonsPanel.SetActive(true);
                m_eventSystem.SetSelectedGameObject(m_controllerButton);
            }
            else if (m_buttonsPanel.activeSelf)
            {
                m_optionsPanel.SetActive(false);
                m_previousPanel.SetActive(true);
                m_eventSystem.SetSelectedGameObject(m_previousButton);
            }
        }
    }

    public void SelectMusic(bool selected)
    {
        m_musicSelected = selected;
    }

    public void SelectEffects(bool selected)
    {
        m_effectsSelected = selected;
    }

    public void ChangeLanguage()
    {
        m_language.isOn = !m_language.isOn;
    }

    public void ChangeMusicVolume()
    {
        AudioManager audio = AudioManager.Instance();
        if(audio != null)
        {
            audio.ChangeMusicVolume(m_musicVolume.value);
        }
    }

    public void ChangeEffectsVolume()
    {
        AudioManager audio = AudioManager.Instance();
        if (audio != null)
        {
            audio.ChangeEffectsVolume(m_effectsVolume.value);
        }
    }

}
