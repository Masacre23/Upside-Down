using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOntriggerCameras : MonoBehaviour {
	BossSceneManager bossSceneManager;
	public int numScene;

	// Use this for initialization
	void Start () {
		bossSceneManager = GameObject.Find ("BossSceneManager").GetComponent<BossSceneManager> ();
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
			bossSceneManager.numScene = numScene;
	}
}
