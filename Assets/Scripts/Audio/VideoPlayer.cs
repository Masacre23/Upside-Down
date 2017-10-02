using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class VideoPlayer : MonoBehaviour {

    public float m_duration;
    public AudioClip m_gameMusic;
    public AudioClip m_cinematicMusic;

    private bool m_isPlaying = false;
	public GameObject menuManager;

	// Use this for initialization
	void Start () {
        float distance = Vector3.Distance(Camera.main.transform.position, gameObject.transform.position);
        float height = 2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * distance;
        float width = height * Screen.width / Screen.height;
        gameObject.transform.localScale = new Vector3(width / 10f, 1.0f, height / 10f);
        AudioManager.Instance().PlayMusic(m_cinematicMusic, 0.5f);
    }
	
	// Update is called once per frame
	void Update () {
		if(m_isPlaying)
        {
            m_duration -= Time.deltaTime;
            if (m_duration <= 0 || (m_duration < 23 && CrossPlatformInputManager.GetButtonDown("Submit")))
            {
                ((MovieTexture)GetComponent<Renderer>().material.mainTexture).Stop();
				menuManager.GetComponent<MainMenuManager> ().async.allowSceneActivation = true;
                AudioManager.Instance().PlayMusic(m_gameMusic, 1.0f);
            }
        }
    }

    public void PlayVideo()
    {
        ((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
        m_isPlaying = true;
    }
}
