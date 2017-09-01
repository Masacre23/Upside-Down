using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScene : MonoBehaviour {
	public int maxTime;
	public int speed;

	float time;

	// Use this for initialization
	void Start () {

	}

	void FixedUpdate () {
		time += Time.deltaTime;
		if(time < maxTime)
			transform.Translate (-transform.up * Time.deltaTime * speed);
	}
}
