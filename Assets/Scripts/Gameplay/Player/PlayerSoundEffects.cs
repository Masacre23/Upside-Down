using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffects : MonoBehaviour {

    static public int SplashWater = 0;
    static public int FootRight = 1;
    static public int FootLeft = 2;
    static public int Jump = 3;

    public AudioClip[] m_soundsEffects;

    private AudioSource m_audio;

	// Use this for initialization
	void Start () {
        m_audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySound(int sound)
    {
        if(m_soundsEffects.Length > sound)
        {
            m_audio.PlayOneShot(m_soundsEffects[sound]);
        }
    }
}
