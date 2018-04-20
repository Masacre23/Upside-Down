using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    public float m_waitTime;
    public AudioClip m_gameOverClip;
    public AudioClip m_mainMenuClip;
    public AudioClip m_gameClip;
    float fadeTime = 2.0f;
    public Image fadePanel;

    private float m_time;
	// Use this for initialization
	void Start ()
    {
        m_time = Time.time;
        fadePanel.gameObject.SetActive(true);
        fadePanel.color = new Color(0f, 0f, 0f, 0f);
        /*AudioManager audioManager = AudioManager.Instance();
        if (audioManager && m_gameOverClip)
            audioManager.PlayMusic(m_gameOverClip, 0.5f);*/
    }
	
	// Update is called once per frame
	void Update () {
        if (!b && Time.time - m_time > m_waitTime - 4f)
            StartCoroutine(Fading());
        if (Time.time - m_time > m_waitTime)
        {
            AudioManager audioManager = AudioManager.Instance();
            if (audioManager && m_mainMenuClip)
                audioManager.PlayMusic(m_mainMenuClip, 0.5f);
            Scenes.LoadScene(Scenes.MainMenu);
        }
        if (CrossPlatformInputManager.GetButtonDown("Cancel"))
        {
            AudioManager audioManager = AudioManager.Instance();
            if (audioManager && m_mainMenuClip)
                audioManager.PlayMusic(m_mainMenuClip, 0.5f);
            HUDManager.ShowGameOverPanel(false);
            HUDManager.ShowWinPanel(false);
            Destroy(GameObject.Find("Data"));
            Destroy(GameObject.Find("AudioManager"));
            Scenes.LoadScene(Scenes.MainMenu);
        }
        if (CrossPlatformInputManager.GetButtonDown("Submit"))
        {
            AudioManager audioManager = AudioManager.Instance();
            if (audioManager && m_mainMenuClip)
                audioManager.PlayMusic(m_gameClip, 0.5f);
            HUDManager.ShowGameOverPanel(false);
            HUDManager.ShowWinPanel(false);
            Scenes.LoadScene(Scenes.Level1);
        }
    }

    bool b = false;
    IEnumerator Fading()
    {
        b = true;
        for (float t = 0.0f; t < fadeTime;)
        {
            t += Time.deltaTime;
            fadePanel.color = new Color(0f, 0f, 0f, t / (fadeTime));
            yield return null;
        }
       /* 
        for (float t = fadeTime; t > 0.0f;)
        {
            t -= Time.deltaTime;
            fadePanel.color = new Color(0f, 0f, 0f, t / (fadeTime));
            yield return null;
        }*/
    }
}
