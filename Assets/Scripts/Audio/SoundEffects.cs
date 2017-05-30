using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour {

    public string[] keys;
    public AudioClip[] sounds;

    private Dictionary<string, AudioClip> m_soundsEffects;

    private AudioSource m_audio;

    private void Awake()
    {
        m_soundsEffects = new Dictionary<string, AudioClip>();
        for(int i = 0; i < keys.Length && i < sounds.Length; i++)
        {
            m_soundsEffects.Add(keys[i], sounds[i]);
        }
    }

    // Use this for initialization
    void Start () {
        m_audio = this.gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySound(string sound)
    {
        if(m_soundsEffects.ContainsKey(sound))
        {
            m_audio.PlayOneShot(m_soundsEffects[sound]);
        }
    }
}
