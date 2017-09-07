using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerActive : MonoBehaviour {

	public GameObject go;

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
			go.SetActive (true);
	}
}
