using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum FallenStoneState
{
    WAIT,
    FALL_DOWN,
}

public class FallenStone : MonoBehaviour {

    public TriggerPlatform trigger;
    public float stronge = 0.01f;

    private FallenStoneState m_state = FallenStoneState.WAIT;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void FixedUpdate () {
        switch (m_state)
        {
            case FallenStoneState.WAIT:
                if (trigger == null || trigger.m_playerDetected)
                    m_state = FallenStoneState.FALL_DOWN;
                break;
            case FallenStoneState.FALL_DOWN:
                Rigidbody rigidBody = GetComponent<Rigidbody>();
                rigidBody.isKinematic = false;
                Vector3 force = /*transform.position +*/ -transform.position.normalized *stronge;
                //rigidBody.MovePosition(force); //* stronge);//, ForceMode.Impulse);
                rigidBody.AddForce(force, ForceMode.Impulse);
                break;
        }
        
    }
}
