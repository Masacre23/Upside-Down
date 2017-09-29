using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundEffects : SoundEffects {
    private string m_resourcesPath = "Audio/Enemy/";
    private string m_walkFile = "Walk";
    private string m_crashFile = "Crash";
    private string m_sleepFile = "Sleep";

    private AudioClip m_walk;
    private AudioClip m_crash;
    private AudioClip m_sleep;
    private bool m_isWalking = false;

    void Start()
    {
        m_walk = Resources.Load<AudioClip>(m_resourcesPath+ m_walkFile);
        m_crash = Resources.Load<AudioClip>(m_resourcesPath + m_crashFile);
        m_sleep = Resources.Load<AudioClip>(m_resourcesPath + m_sleepFile);
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

    public void PlaySleep()
    {
        base.PlaySoundLoop(m_sleep);
    }

    public void StopSleep()
    {
        base.StopSoundLoop();
    }

    public void PlayCrash()
    {
        base.PlaySound(m_crash);
    }
}
