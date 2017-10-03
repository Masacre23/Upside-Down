using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLightRange : MonoBehaviour {

	public Light lt;
	public int range;

	void Start () {
		lt.range = range;
	}
	
	// Update is called once per frame
	void Update () {
		lt.range = range;
	}
}
