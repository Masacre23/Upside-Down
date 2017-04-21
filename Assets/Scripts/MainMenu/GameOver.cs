using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

    public float m_waitTime;

    private float m_time;
	// Use this for initialization
	void Start () {
        m_time = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - m_time > m_waitTime)
        {
            Application.LoadLevel(0);
        }
	}
}
