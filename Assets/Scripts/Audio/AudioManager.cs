using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public float m_soundVolume = 1.0f;

    AudioSource musicAudioSource1;
    AudioSource musicAudioSource2;
    static AudioManager instance;
    bool isMute;
    float m_musicVolume = 0.1f;
    bool m_audioSource1IsPlaying;
    bool m_changeAudioSource = false;
    float m_secondChange;

    private void Awake()
    {       
    }

    void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        AudioSource[] audioSources = GetComponents<AudioSource>();
        musicAudioSource1 = audioSources[0];
        musicAudioSource2 = audioSources[1];
        musicAudioSource1.Play();
        m_audioSource1IsPlaying = true;
    }

    private void Update()
    {
        this.transform.position = Camera.main.transform.position;
        if(m_changeAudioSource)
        {
            AudioSource audioSource1 = m_audioSource1IsPlaying ? musicAudioSource1 : musicAudioSource2;
            AudioSource audioSource2 = m_audioSource1IsPlaying ? musicAudioSource2 : musicAudioSource1;
            audioSource1.volume += Time.deltaTime / m_secondChange * m_musicVolume;
            audioSource2.volume -= Time.deltaTime / m_secondChange * m_musicVolume;
            if(audioSource2.volume <= 0 || audioSource1.volume >= m_musicVolume)
            {
                audioSource2.Stop();
                audioSource2.volume = 0;
                audioSource1.volume = m_musicVolume;
                m_changeAudioSource = false;
            }

        }
    }

    public static AudioManager Instance()
    {
        return instance;
    }

    public void PlayMusic(AudioClip music, float timeChanged)
    {
        AudioSource audioSource = m_audioSource1IsPlaying ? musicAudioSource2 : musicAudioSource1;
        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.volume = 0;
        audioSource.Play();
        m_secondChange = timeChanged;
        m_audioSource1IsPlaying = !m_audioSource1IsPlaying;
        m_changeAudioSource = true;
    }

    public void ChangeMusicVolume(float volume)
    {
        m_musicVolume = volume;
        AudioSource audioSource = m_audioSource1IsPlaying ? musicAudioSource1 : musicAudioSource2;
        audioSource.volume = m_musicVolume;
    }

    public void ChangeEffectsVolume(float volume)
    {
        m_soundVolume = volume;
    }
}
