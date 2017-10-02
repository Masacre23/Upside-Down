using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanBossSoundEffects : SoundEffects {

    public AudioClip m_talk;
    public AudioClip m_laugth;
    public AudioClip m_walk;
    public AudioClip m_hit;
    public AudioClip m_pain;
    private bool m_isWalking = false;

    void Start()
    {
    }

    public void PlayTalk()
    {
        base.PlaySoundLoop(m_talk);
    }

    public void PlayLaught()
    {
        base.PlaySoundLoop(m_laugth);
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

    public void PlayPain()
    {
        base.PlaySoundLoop(m_pain);
    }

    public void StopLoop()
    {
        base.StopSoundLoop();
    }

    public void PlayHit()
    {
        base.PlaySound(m_hit);
    }
}
