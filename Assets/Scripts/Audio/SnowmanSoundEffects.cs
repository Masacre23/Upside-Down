using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanSoundEffects : SoundEffects {

    public AudioClip m_talk;

    void Start()
    {
    }

    public void PlayTalk()
    {
        base.PlaySoundLoop(m_talk);
    }

    public void StopTalk()
    {
        base.StopSoundLoop();
    }
}
