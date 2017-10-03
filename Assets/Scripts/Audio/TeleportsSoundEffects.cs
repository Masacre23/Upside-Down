using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportsSoundEffects : SoundEffects {

    public static string m_resourcesPath = "Audio/Teleport/";
    public static string m_idleFile = "Idle";
    public static string m_passFile = "Pass";

    private AudioClip m_idle;
    private AudioClip m_pass;

    void Start()
    {
        m_idle = Resources.Load<AudioClip>(m_resourcesPath + m_idleFile);
        m_pass = Resources.Load<AudioClip>(m_resourcesPath + m_passFile);
    }

    public void PlayIdle()
    {
        base.PlaySoundLoop(m_idle);
    }

    public void PlayPass()
    {
        base.PlaySound(m_pass);
    }
}
