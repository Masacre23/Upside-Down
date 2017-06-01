using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

    public float m_waitTime;
    public AudioClip m_gameOverClip;
    public AudioClip m_mainMenuClip;

    private float m_time;
	// Use this for initialization
	void Start ()
    {
        m_time = Time.time;

        AudioManager audioManager = AudioManager.Instance();
        if (audioManager && m_gameOverClip)
            audioManager.PlayMusic(m_gameOverClip, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - m_time > m_waitTime)
        {
            AudioManager audioManager = AudioManager.Instance();
            if (audioManager && m_mainMenuClip)
                audioManager.PlayMusic(m_mainMenuClip, 0.5f);
            Scenes.LoadScene(Scenes.MainMenu);
        }
	}
}
