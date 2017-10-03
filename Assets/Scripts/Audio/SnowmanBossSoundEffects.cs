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
    private bool m_isLaughting = false;

    void Start()
    {
    }

    public void PlayTalk()
    {
        m_isLaughting = false;
        base.PlaySoundLoop(m_talk);
    }

    public void PlayLaught()
    {
        m_isLaughting = true;
        base.PlaySoundLoop(m_laugth);
    }


    public void PlayWalk()
    {
        m_isLaughting = false;
        if (!m_isWalking)
        {
            m_isWalking = true;
            base.PlaySoundLoop(m_walk);
        }
    }

    public void StopWalk()
    {
        m_isLaughting = false;
        if (m_isWalking)
        {
            m_isWalking = false;
            base.StopSoundLoop();
        }
    }

    public void PlayPain()
    {
        m_isLaughting = false;
        base.PlaySoundLoop(m_pain);
    }

    public void StopLoop()
    {
        base.StopSoundLoop();
    }

    public void StopLaught()
    {
        if (m_isLaughting)
        {
            base.StopSoundLoop();
            m_isLaughting = false;
        }
    }

    public void PlayHit()
    {
        base.PlaySound(m_hit);
    }
}
