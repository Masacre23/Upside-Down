using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum FishState
{
    STOP,
    WAIT,
    MOVE,
}

public class ParabolicFish : MonoBehaviour {

    public Transform m_initialPosition;
    public float m_XSpeed;
    public float m_YSpeed;
    public float m_gravity;
    public float m_wait;

    private Vector3 m_initialVector;
    private float m_aceleration;
    private float m_lastSpeed;
    private FishState m_state;
    private float m_waiting;

    // Use this for initialization
    void Start () {
        m_initialVector = m_initialPosition.position;
        m_aceleration = m_gravity / 2;
        m_lastSpeed = m_YSpeed;
        m_state = FishState.WAIT;
        m_waiting = 0;
    }
	
	// Update is called once per frame
	void Update () {
        switch (m_state)
        {
            case FishState.WAIT:
                m_waiting += Time.deltaTime;
                if(m_waiting >= m_wait)
                {
                    m_state = FishState.MOVE;
                    m_waiting = 0;
                }
                break;
            case FishState.MOVE:
                if (m_lastSpeed <= -m_YSpeed -20)
                {
                    transform.position = m_initialVector;
                    m_lastSpeed = m_YSpeed;
                    m_state = FishState.WAIT;
                    SoundEffects sound = GetComponent<SoundEffects>();
                    if(sound != null)
                    {
                        sound.PlaySound("Bubbles");
                    }
                }
                else
                {
                    Vector3 newPosition = Vector3.zero;
                    newPosition.x = m_XSpeed * Time.deltaTime;
                    newPosition.y = m_lastSpeed * Time.deltaTime - m_aceleration * Time.deltaTime * Time.deltaTime;
                    m_lastSpeed = m_lastSpeed - m_gravity * Time.deltaTime;

                    transform.Translate(newPosition);
                }
                break;
        }
        
	}
}
