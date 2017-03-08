using UnityEngine;
using System.Collections;

public class ChangeGravity : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider col){
		col.gameObject.GetComponent<GravityObject> ().SetDirection (-Camera.main.transform.forward);
	}
}
