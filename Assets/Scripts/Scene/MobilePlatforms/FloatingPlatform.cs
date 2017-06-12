using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Vector3 distance = other.transform.position - this.transform.position;
            this.transform.Rotate(new Vector3(0.0f, distance.x * speed * Time.deltaTime, 0.0f));
        }
    }
}
