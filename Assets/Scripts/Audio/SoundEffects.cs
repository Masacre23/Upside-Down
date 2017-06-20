using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour {

    public string[] keys;
    public AudioClip[] sounds;

    private Dictionary<string, AudioClip> m_soundsEffects;

    private AudioSource m_audio;

    private void Awake()
    {
        m_soundsEffects = new Dictionary<string, AudioClip>();
        for(int i = 0; i < keys.Length && i < sounds.Length; i++)
        {
            m_soundsEffects.Add(keys[i], sounds[i]);
        }

        m_audio = GetComponent<AudioSource>();
        if (!m_audio)
        {
            m_audio = this.gameObject.AddComponent<AudioSource>();
        }
           
    }

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySound(string sound)
    {
        if(m_soundsEffects.ContainsKey(sound))
        {
            m_audio.PlayOneShot(m_soundsEffects[sound], 1.0f);
        }
    }

    public void PlaySoundLoop(string sound)
    {
        if (m_soundsEffects.ContainsKey(sound))
        {
            m_audio.clip = m_soundsEffects[sound];
            m_audio.loop = true;
            m_audio.Play();
            m_audio.volume = 1.0f;
        }
    }

    public void StopSoundLoop(string sound)
    {
        if (m_soundsEffects.ContainsKey(sound))
        {
            m_audio.Stop();
        }
    }
}
