using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLightFace : MonoBehaviour {

	public GameObject lt;
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.transform.tag == "Player")
			lt.transform.parent = col.transform;
	}

	void OnTriggerExit(Collider col)
	{
		if(col.transform.tag == "Player")
			lt.transform.parent = null;
	}
}
