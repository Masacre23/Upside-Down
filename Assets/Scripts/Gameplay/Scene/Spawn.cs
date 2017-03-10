using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {
	public GameObject bottlePrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("i")) {
			Instantiate (bottlePrefab, transform.position, transform.rotation);
		}
	}
}
