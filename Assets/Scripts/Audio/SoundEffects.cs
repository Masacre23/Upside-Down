using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour {

    public static string m_resourcesWaterPath = "Audio/Water/";

    private AudioClip m_splash;

    private Dictionary<string, AudioClip> m_soundsEffects;

    private AudioSource m_audio;

    private void Awake()
    {
        m_audio = GetComponent<AudioSource>();
        if (!m_audio)
        {
            m_audio = this.gameObject.AddComponent<AudioSource>();
        }
        m_splash = Resources.LoadAll<AudioClip>(m_resourcesWaterPath)[0];
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

    public void PlaySplash()
    {
        AudioManager manager = AudioManager.Instance();
        float volume = 1.0f;
        if (manager != null)
            volume = manager.m_soundVolume;
        m_audio.PlayOneShot(m_splash, volume);
    }

}
