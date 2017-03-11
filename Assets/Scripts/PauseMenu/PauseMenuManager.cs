using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour {

    public KeyCode mPauseKey;
    public GameObject mPausePanel;

    private bool mIsPaused = false;
	// Use this for initialization
	void Start () {
        mPausePanel.SetActive(mIsPaused);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(mPauseKey)){
            Paused();
        }
	}

    void Paused()
    {
        mIsPaused = !mIsPaused;
        mPausePanel.SetActive(mIsPaused);
        Time.timeScale = mIsPaused ? 0 : 1;
    }

    public void Resume()
    {
        Paused();
    }

    public void Quit()
    {
        Application.LoadLevel(1);
    }
}
