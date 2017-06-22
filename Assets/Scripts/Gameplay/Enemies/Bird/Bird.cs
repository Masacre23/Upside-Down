using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

    Vector3 v;
    public Transform point;
    public float speed = 20.0f;

	void Start () {
        v = transform.position - point.position;
	}
	
	// Update is called once per frame
	void Update () {
		//v = Quaternion.AngleAxis(Time.deltaTime * speed, Vector3.forward) * v;
      //  transform.position = point.position + v;

        transform.RotateAround(point.position, transform.up, 20 * Time.deltaTime);
    }
}
