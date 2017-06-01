using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class VideoPlayer : MonoBehaviour {

    public float m_duration;
    public AudioClip m_gameMusic;

    private bool m_isPlaying = false;
	// Use this for initialization
	void Start () {
        float distance = Vector3.Distance(Camera.main.transform.position, gameObject.transform.position);
        float height = 2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * distance;
        float width = height * Screen.width / Screen.height;
        gameObject.transform.localScale = new Vector3(width / 10f, 1.0f, height / 10f);
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
