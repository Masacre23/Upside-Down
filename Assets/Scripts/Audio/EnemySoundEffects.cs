using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundEffects : SoundEffects {
    public static string m_resourcesPath = "Audio/Enemy/";
    public static string m_walkPath = "Walk/";
    public static string m_crashPath = "Crash/";

    private AudioClip m_walk;
    private AudioClip m_crash;
    private bool m_isWalking = false;

    void Start()
    {
        m_walk = Resources.LoadAll<AudioClip>(m_resourcesPath+ m_walkPath)[0];
        m_crash = Resources.LoadAll<AudioClip>(m_resourcesPath + m_crashPath)[0];
    }

    public void PlayWalk()
    {
        if (!m_isWalking)
        {
            m_isWalking = true;
            base.PlaySoundLoop(m_walk);
        }
    }

    public void StopWalk()
    {
        if (m_isWalking)
        {
            m_isWalking = false;
            base.StopSoundLoop();
        }
    }

    public void PlayCrash()
    {
        base.PlaySound(m_crash);
    }
}
