using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityObject : MonoBehaviour {
	
	public Vector3 direction;
	public float gravity = 10;

	// Use this for initialization
	void Start () {
		direction = transform.up;
	}
	
	void FixedUpdate(){
		GetComponent<Rigidbody> ().AddForce (-gravity * GetComponent<Rigidbody> ().mass * direction);
	}

	public void SetDirection(Vector3 dir){
		direction = dir;
	}
}
