﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour {

    public Slider m_musicVolume;
    public Slider m_effectsVolume;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
