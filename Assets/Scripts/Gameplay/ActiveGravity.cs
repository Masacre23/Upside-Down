using UnityEngine;
using System.Collections;

public class ActiveGravity : MonoBehaviour {
	public GameObject campo;
	bool b;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("q")) {
			b = !b;
			campo.SetActive (b);
		}
	}
}
