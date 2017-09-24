using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffects : SoundEffects {

    public int m_numberFootStepWalk = 0;
    public AudioClip[] m_rigthFootStepSnowWalk;
    public AudioClip[] m_leftFootStepSnowWalk;
    public AudioClip[] m_rigthFootStepSnowRun;
    public AudioClip[] m_leftFootStepSnowRun;
    public AudioClip m_rightFootStepCloud;
    public AudioClip m_leftFootStepCloud;

    private int m_footStepSnowWalkIndex = 0;
    private int m_footStepSnowRunIndex = 0;

    public void PlayFootStepWalk(bool right)
    {
        AudioClip footStep = m_leftFootStepSnowWalk[m_footStepSnowWalkIndex];
        if (right)
        {
            m_footStepSnowWalkIndex = Random.Range(0, m_numberFootStepWalk);
            footStep = m_rigthFootStepSnowWalk[m_footStepSnowWalkIndex];
        }
        base.PlaySound(footStep);
    }

}
