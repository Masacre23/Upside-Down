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

    public void PlaySound(AudioClip clip)
    {
        AudioManager manager = AudioManager.Instance();
        float volume = 1.0f;
        if (manager != null)
            volume = manager.m_soundVolume;
        m_audio.PlayOneShot(clip, volume);
    }

    public void PlaySoundLoop(AudioClip clip)
    {
        AudioManager manager = AudioManager.Instance();
        float volume = 1.0f;
        if (manager != null)
            volume = manager.m_soundVolume;
        m_audio.clip = clip;
        m_audio.loop = true;
        m_audio.Play();
        m_audio.volume = volume;
    }

    public void StopSoundLoop()
    {
        m_audio.Stop();
    }

    public void PlaySound(string sound)
    {
        AudioManager manager = AudioManager.Instance();
        float volume = 1.0f;
        if (manager != null)
            volume = manager.m_soundVolume;
        if (m_soundsEffects.ContainsKey(sound))
        {
            m_audio.PlayOneShot(m_soundsEffects[sound], volume);
        }
    }

    public void PlaySoundLoop(string sound)
    {
        AudioManager manager = AudioManager.Instance();
        float volume = 1.0f;
        if (manager != null)
            volume = manager.m_soundVolume;
        if (m_soundsEffects.ContainsKey(sound))
        {
            m_audio.clip = m_soundsEffects[sound];
            m_audio.loop = true;
            m_audio.Play();
            m_audio.volume = volume;
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
