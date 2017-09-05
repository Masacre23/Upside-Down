using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlocks : MonoBehaviour {

	public int maxTime;
	public int speed;

	float time;
	public Vector3 direcction;
	public bool down = false;

	// Use this for initialization
	void Start () {

	}

	void FixedUpdate () {
		if (!down) 
		{
			time += Time.deltaTime;
			if (time < maxTime)
				transform.Translate (direcction * Time.deltaTime * speed);
		} else 
		{
			time -= Time.deltaTime;
			if (time > 0)
				transform.Translate (-direcction * Time.deltaTime * speed);
		}
	}
}

