using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPrueba : MonoBehaviour {

    Vector3 v;
    public Transform point;
    public float speed = 20.0f;

	enum state
	{
		IDLE,
		ATTACKING
	};

	state bird_state;
	Vector3 target;

	void Start () {
        v = transform.position - point.position;
		bird_state = state.IDLE;
	}
	
	// Update is called once per frame
	void Update () {
		switch(bird_state)
		{
		case state.IDLE:
			transform.RotateAround (point.position, transform.up, 20 * Time.deltaTime);
			break;

		case state.ATTACKING:
			transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.LookRotation( target - transform.position ), Time.deltaTime );
			transform.position += transform.forward * speed * Time.deltaTime;
			break;
		}
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			bird_state = state.ATTACKING;
			target = other.gameObject.transform.position;
		}
	}
}
