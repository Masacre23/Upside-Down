using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSounds
{
    private AudioClip[] m_footsteeps;
    private AudioClip m_jump;
    private AudioClip m_fall;

    private int m_index = 0;

    public TerrainSounds(string path)
    {
        m_footsteeps = Resources.LoadAll<AudioClip>(PlayerSoundEffects.m_resourcesPath + PlayerSoundEffects.m_footsteepPath + path + "/");
        m_jump = Resources.LoadAll<AudioClip>(PlayerSoundEffects.m_resourcesPath + PlayerSoundEffects.m_jumpPath + path + "/")[0];
        m_fall = Resources.LoadAll<AudioClip>(PlayerSoundEffects.m_resourcesPath + PlayerSoundEffects.m_fallPath + path + "/")[0];
    }

    public AudioClip GetFootSteep()
    {
        int index = m_index;
        while(index == m_index)
        {
            m_index = Random.Range(0, m_footsteeps.Length);
        }
        return m_footsteeps[m_index];
    }

    public AudioClip GetJump()
    {
        return m_jump;
    }

    public AudioClip GetFall()
    {
        return m_fall;
    }
}

public class PlayerSoundEffects : SoundEffects {

    public static string m_resourcesPath = "Audio/Player/";
    public static string m_footsteepPath = "Footsteeps/";
    public static string m_jumpPath = "Jumps/";
    public static string m_fallPath = "Fall/";
    private string m_collectableFile = "Collectable";
    private string m_gameOverFailFile = "GameOver";

    private string[] m_terrains = new string[] { "Snow", "Cloud" };
    private Dictionary<string, TerrainSounds> m_fxSounds = new Dictionary<string, TerrainSounds>();

    private AudioClip m_collectable;
    private AudioClip m_gameOverFail;

    private int m_terrainIndex = 0;

    void Start()
    {
        for(int i = 0; i < m_terrains.Length; i++)
        {
            m_fxSounds.Add(m_terrains[i], new TerrainSounds(m_terrains[i]));
        }
        m_collectable = Resources.Load<AudioClip>(m_resourcesPath + m_collectableFile);
        m_gameOverFail = Resources.Load<AudioClip>(m_resourcesPath + m_gameOverFailFile);
    }

    public void PlayFootStep()
    {
        base.PlaySound(m_fxSounds[m_terrains[m_terrainIndex]].GetFootSteep());
    }

    public void PlayJump()
    {
        base.PlaySound(m_fxSounds[m_terrains[m_terrainIndex]].GetJump());
    }

    public void PlayFall()
    {
        base.PlaySound(m_fxSounds[m_terrains[m_terrainIndex]].GetFall());
    }

    public void PlayCollectable()
    {
        base.PlaySound(m_collectable);
    }
 
    public void PlayGameOver()
    {
        AudioManager.Instance().PlayMusicWithoutLoop(m_gameOverFail, 0.5f);
    }
}
