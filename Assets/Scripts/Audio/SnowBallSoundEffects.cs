using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallSoundEffects : SoundEffects {

    public static string m_resourcesPath = "Audio/Snowball/";
    public static string m_crashPath = "Crash/";

    private AudioClip m_crash;

    void Start()
    {
        m_crash = Resources.LoadAll<AudioClip>(m_resourcesPath + m_crashPath)[0];
    }

    public void PlayCrash()
    {
        base.PlaySound(m_crash);
    }
}
