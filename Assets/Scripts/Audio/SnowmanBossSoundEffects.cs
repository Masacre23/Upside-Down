using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanBossSoundEffects : SoundEffects {

    public AudioClip m_talk;
    public AudioClip m_laugth;
    public AudioClip m_walk;
    public AudioClip m_hit;

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
        base.PlaySoundLoop(m_walk);
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
