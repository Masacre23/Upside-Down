using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class VideoPlayer : MonoBehaviour {

    public float m_duration;
    public AudioClip m_gameMusic;

    private bool m_isPlaying = false;
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		if(m_isPlaying)
        {
            m_duration -= Time.deltaTime;
            if (m_duration <= 0 || (m_duration < 23 && CrossPlatformInputManager.GetButtonDown("Jump")))
            {
                ((MovieTexture)GetComponent<Renderer>().material.mainTexture).Stop();
                Scenes.LoadScene(Scenes.Level1);
                AudioManager.Instance().PlayMusic(m_gameMusic, 4.0f);
            }
        }
    }

    public void PlayVideo()
    {
        ((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
        m_isPlaying = true;
    }
}
