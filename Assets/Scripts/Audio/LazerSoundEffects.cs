using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerSoundEffects : SoundEffects {

    public static string m_resourcesPath = "Audio/Lazer/";
    public static string m_lazerFile = "Lazer";

    private AudioClip m_lazer;

    void Start()
    {
        m_lazer = Resources.Load<AudioClip>(m_resourcesPath + m_lazerFile);
    }

    public void PlayLazer()
    {
        base.PlaySoundLoop(m_lazer);
    }

    public void StopLazer()
    {
        base.StopSoundLoop();
    }
}
